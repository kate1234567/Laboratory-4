namespace FamilyTree.DAL
{
    public class Human
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public DateTime BirthDay { get; set; }
        public GenderEnum Gender { get; set; }
        public List<Human>? Parents { get; set; } = new List<Human>();
        public List<Human>? Childrens { get; set; } = new List<Human>();
        public int FamilyTreeId { get; set; }

        public Human()
        {

        }

        public Human(string fio, DateTime birthDay, GenderEnum gender, int familyTreeId)
        {
            FIO = fio;
            BirthDay = birthDay;
            Gender = gender;
            FamilyTreeId = familyTreeId;
        }

        public enum GenderEnum
        {
            Male = 0,
            Female = 1
        }

        public override string ToString()
        {
            return $"{FIO} {BirthDay.ToShortDateString()}";
        }
    }
}
