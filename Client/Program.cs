using Client.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();
            // Add http client to call to api with base address
            var httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            httpClient.BaseAddress = new Uri(builder.Configuration["apiEndpoint"]);
            builder.Services.AddSingleton(httpClient);
            HttpUtils.Initialize(httpClient);

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                var endPoint = context.Request.Path;
                Console.WriteLine(endPoint);
                await next(context);
            });
            app.Use(async (context, next) =>
            {
                var endPoint = context.Request.Path;
                if(Regex.IsMatch(endPoint.ToString().ToLower(), "login") 
                || Regex.IsMatch(endPoint.ToString().ToLower(), "register")
                || Regex.IsMatch(endPoint.ToString().ToLower(), "forgotpassword")
                || Regex.IsMatch(endPoint.ToString().ToLower(), "signout")
                //|| Regex.IsMatch(endPoint.ToString().ToLower(), "")
                )
                {
                    await next(context);
                } else
                {
                    var accessToken = context.Request.Cookies["accessToken"];
                    //var options = new RewriteOptions().AddRedirect("", "/Login", StatusCodes.Status308PermanentRedirect);
                    if (accessToken == null)
                    {
                        context.Response.Redirect("/login");
                    } else
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(accessToken);
                        var tokenS = jsonToken as JwtSecurityToken;

                        var injectedHttpClient = app.Services.GetService<HttpClient>();
                        //if (injectedHttpClient.DefaultRequestHeaders.Authorization == null)
                        //{
                        //    injectedHttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                        //}

                        //
                        if (tokenS.ValidTo < DateTime.UtcNow)
                        {
                            string refreshToken = context.Request.Cookies["refreshToken"];

                            // Create an instance of HttpClientHandler
                            var httpClientHandler = new HttpClientHandler();

                            // Configure the HttpClientHandler to use cookies
                            httpClientHandler.UseCookies = true;
                            httpClientHandler.CookieContainer = new CookieContainer();

                            // Get the cookies from the client's request
                            var clientCookies = context.Request.Cookies;
                            foreach (var cookie in clientCookies)
                            {
                                // Add each cookie to the CookieContainer
                                httpClientHandler.CookieContainer.Add(new Uri("https://localhost:7038"), new Cookie(cookie.Key, cookie.Value));
                            }

                            // Make a request to your API's refresh token endpoint
                            HttpClient httpClient = new HttpClient(httpClientHandler);
                            var response = await httpClient.PostAsync("https://localhost:7038/refresh-token", null);

                            if (response.IsSuccessStatusCode)
                            {
                                //update refresh token to cookie
                                JWTUtils.SetRefreshToken(response, context.Response);

                                // Extract the new access token and refresh token from the response
                                var responseString = await response.Content.ReadAsStringAsync();
                                System.Text.Json.JsonElement anonymousObject = (System.Text.Json.JsonElement)JsonSerializer.Deserialize<object>(responseString);
                                string newAccessToken = anonymousObject.GetString("accessToken");
                                Console.WriteLine(newAccessToken);
                                JWTUtils.SetAccessToken(context.Response, newAccessToken);

                                //add new access token to injected http client
                                injectedHttpClient.DefaultRequestHeaders.Remove("Authorization");
                                injectedHttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + newAccessToken);

                                await next(context);
                            }
                            else
                            {
                                var responseString = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("refreshTokenFailed: "+ responseString);
                                context.Response.Redirect("/login");
                            }
                        } else
                        {
                            injectedHttpClient.DefaultRequestHeaders.Remove("Authorization");
                            injectedHttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                            await next(context);
                        }
                    }

                }
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}/{episodeId?}");

            app.Run();
        }
    }
}