namespace MyPet.Areas.Services.Abstractions
{
    public interface ITimeDifference
    {
        public string GetDifference(DateTime dateTime);
        public string GetDifferenceEng(DateTime _dateTime);


    }
}
