using APIProject.DTO.Comment;
using AutoMapper;
using BusinessObjects;

namespace APIProject.Mapping
{
    public class TimeSpanFormatterConverter : IValueResolver<Comment, GetCommentResponseDTO, string>
    {
        //public string Convert(TimeSpan sourceMember, ResolutionContext context)
        //{
        //    string format = (sourceMember.Days > 0 ? "d'd '" : "") +
        //               (sourceMember.Hours > 0 ? "h'h '" : "") +
        //               (sourceMember.Minutes > 0 ? "m'm '" : "") +
        //               (sourceMember.Seconds > 0 ? "s's'" : "");

        //    // Format the TimeSpan using the custom format string
        //    string formattedString = sourceMember.ToString(format);

        //    // If the formatted string is empty, return "0s"
        //    return !string.IsNullOrEmpty(formattedString) ? formattedString : "0s";
        //}
        public string Resolve(Comment source, GetCommentResponseDTO destination, string destMember, ResolutionContext context)
        {
            TimeSpan time = DateTime.Now - source.CommentedDate;
            string format = (time.Days > 0 ? "d'd '" : "") +
                       (time.Hours > 0 ? "h'h '" : "") +
                       (time.Minutes > 0 ? "m'm '" : "") +
                       (time.Seconds >= 0 ? "s's'" : "");

            // Format the TimeSpan using the custom format string
            string formattedString = time.ToString(format);

            // If the formatted string is empty, return "0s"
            return !string.IsNullOrEmpty(formattedString) ? formattedString : "0s";
        }
    }
}
