using Proj1.Models;
Console.WriteLine("Witaj");

var context = new Proj1Context();

int wybór = 0;
while (wybór != 10)
{
    Console.WriteLine("1-Dodaj aktywo");
    Console.WriteLine("2-Usuń aktywo");
    Console.WriteLine("3-Dodaj notowanie");
    Console.WriteLine("4-Usuń notowanie");
    Console.WriteLine("5-Wyświetl aktywa");
    Console.WriteLine("6-Wyświetl notowania");
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
        case 3:
            context.DodajNotowanie();
            context.SaveChanges();
            break;
        case 4:
            context.UsunNotowanie();
            context.SaveChanges();
            break;
        case 5:
            context.WyswietlAktywa();
            break;
        case 6:
            context.WyswietlNotowania();
            break;
    }
}
