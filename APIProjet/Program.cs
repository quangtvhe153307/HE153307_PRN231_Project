using APIProject.DTO.Category;
using APIProject.DTO.Comment;
using APIProject.DTO.Movie;
using APIProject.DTO.Role;
using APIProject.DTO.Transaction;
using APIProject.DTO.User;
using APIProject.Mapping;
using APIProject.Util;
using AutoMapper;
using BusinessObjects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using Newtonsoft.Json.Linq;
using Repository.IRepository;
using Repository.Repository;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace APIProjet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddOData(options
                => options.Select().Filter().OrderBy().Expand().SetMaxTop(null)
                .AddRouteComponents("odata", GetEdmModel()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IRefreshtokenRepository, RefreshtokenRepository>();
            builder.Services.AddSingleton<IJWTUtils, JWTUtils>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    //ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:secret"]))
                };

                // Set the event handler for token validation failure
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        if (context.SecurityToken.ValidTo < DateTime.UtcNow)
                        {
                            string refreshToken = context.Request.Cookies["refreshToken"];

                            // Create an instance of HttpClientHandler
                            var handler = new HttpClientHandler();

                            // Configure the HttpClientHandler to use cookies
                            handler.UseCookies = true;
                            handler.CookieContainer = new CookieContainer();

                            // Get the cookies from the client's request
                            var clientCookies = context.Request.Cookies;
                            foreach (var cookie in clientCookies)
                            {
                                // Add each cookie to the CookieContainer
                                handler.CookieContainer.Add(new Uri("https://localhost:7038"), new Cookie(cookie.Key, cookie.Value));
                            }

                            // Make a request to your API's refresh token endpoint
                            HttpClient httpClient = new HttpClient(handler);
                            var response = await httpClient.PostAsync("https://localhost:7038/refresh-token", null);

                            if (response.IsSuccessStatusCode)
                            {
                                //update refresh token to cookie
                                var refreshTokenCookie = response.Headers.GetValues("Set-Cookie");
                                string cookie = Uri.UnescapeDataString(refreshTokenCookie.ToArray()[0]);
                                int indexOfFirstSemiColon = cookie.IndexOf(";");
                                int indexOfFirstEqual = cookie.IndexOf("=");

                                string token = cookie.Substring(indexOfFirstEqual + 1, indexOfFirstSemiColon - indexOfFirstEqual - 1);

                                Console.WriteLine("success");
                                // append cookie with refresh token to the http response
                                var cookieOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Expires = DateTime.UtcNow.AddDays(7),
                                    IsEssential = true
                                };
                                context.Response.Cookies.Append("refreshToken", token, cookieOptions);


                                // Extract the new access token and refresh token from the response
                                var responseString = await response.Content.ReadAsStringAsync();
                                System.Text.Json.JsonElement anonymousObject = (System.Text.Json.JsonElement)JsonSerializer.Deserialize<object>(responseString);
                                string newAccessToken = anonymousObject.GetString("accessToken");
                            } else
                            {
                                context.Fail("Failed to refresh access token");
                            }

                        }

                    }
                };
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            //app.Use(async (context, next) =>
            //{
            //    await next();

            //    Console.WriteLine("quang");
            //    if (context.Response.StatusCode == 401 && context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            //    {
            //        var refreshToken = context.Response.Body;

            //        //// Use the token refresh service to refresh the access token
            //        //var newAccessToken = await _tokenRefreshService.RefreshAccessToken(refreshToken);

            //        //if (newAccessToken != null)
            //        //{
            //        //    // Update the access token in the response headers
            //        //    context.Response.Headers["Authorization"] = $"Bearer {newAccessToken}";

            //        //    // Retry the original request with the new access token
            //        //    await next(context);
            //        //}
            //    }
            //});
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.MapControllers();

            app.Run();
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<GetUserResponseDTO>("Users");
            builder.EntitySet<GetCategoryResponseDTO>("Categories");
            builder.EntitySet<GetCommentResponseDTO>("Comments");
            builder.EntitySet<GetMovieResponseDTO>("Movies");
            builder.EntitySet<GetRoleResponseDTO>("Roles");
            builder.EntitySet<GetTransactionResponseDTO>("Transactions");
            return builder.GetEdmModel();
        }
    }
}