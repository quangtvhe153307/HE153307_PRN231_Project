namespace APIProject.Util
{
    public class DateTimeUtils
    {
        public static DateTime GetStartDateRanking(int type)
        {
            switch (type)
            {
                case 1:
                    return DateTime.Today;
                case 2:
                    return DateTime.Today.AddDays(-6);
                case 3:
                    return DateTime.Today.AddDays(-29);
                default: 
                    return DateTime.MinValue;
            }
        }        
        public static DateTime GetEndDateRanking()
        {
            return DateTime.Today.AddDays(1);
        }
    }
}
