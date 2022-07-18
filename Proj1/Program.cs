using Proj1.Models;
Console.WriteLine("Witaj");

var context = new Proj1Context();

int wybór = 0;
while (wybór != 5)
{
    Console.WriteLine("1-Dodaj aktywo");
    Console.WriteLine("2-Usuń aktywo");
    Console.Write($"Wybierz co chcesz zrobić: ");
    wybór = Int32.Parse(Console.ReadLine());
    Console.WriteLine();

    switch (wybór)
    {
        case 1:
            context.DodajAktywo();
            context.SaveChanges();
            break;
        case 2:
            context.UsunAktywo();
            context.SaveChanges();
            break;
        default:
            break;
    }
}
