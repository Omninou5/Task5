namespace Task5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<DateTime>> VacationDictionary = Initialize();
            List<string> Weekends = new List<string>() { "Saturday", "Sunday" };

            // Список дат отпусков всех сотрудников (168 записей + выходные)
            List<DateTime> AllVacations = new List<DateTime>();

            foreach (var Vacation in VacationDictionary)
            {
                Random gen = new Random();

                // Установка начала и конца текущего года
                DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime end = new DateTime(DateTime.Today.Year, 12, 31);

                // Список дат отпусков текущего сотрудника (28 записей + выходные)
                List<DateTime> VacationsEmployee;

                // Присвоение ссылки из списка словаря в переменную списка дат отпуска текущего работника
                VacationsEmployee = Vacation.Value;

                // Продолжительность всего отпуска
                int allVacationCount = 28;


                while (allVacationCount > 0)
                {
                    // Определение количества дней за период от начала до конца года
                    int range = (end - start).Days;

                    // Установка начальной даты отпуска (случайным образом)
                    DateTime startDate = start.AddDays(gen.Next(range));


                    int difference = 0;
                    List<DateTime> CurrentVacationEmployee = new List<DateTime>();


                    if (Weekends.Contains(startDate.DayOfWeek.ToString()))
                    {
                        continue;
                    }


                    // Продолжительность текущего отпуска (можно взять от 7 до 28 дней)
                    int currentVacationCount = gen.Next(7, 29);

                    // Если продолжительность отпуска больше оставшейся части, то текущий отпуск уменьшается
                    if (allVacationCount <= currentVacationCount)
                    {
                        currentVacationCount = allVacationCount;
                    }
                    difference = currentVacationCount;


                    // Текущая дата
                    DateTime currentDate = startDate;

                    // Перебор каждого дня отпуска
                    while (currentVacationCount > 0)
                    {
                        AddWeekendDay(currentDate, ref currentVacationCount);

                        CurrentVacationEmployee.Add(currentDate);

                        // Инкремент текущей даты
                        currentDate = currentDate.AddDays(1);

                        if (currentDate > end)
                        {
                            difference = difference - currentVacationCount;
                            currentVacationCount = 0;
                            continue;
                        }

                        currentVacationCount = currentVacationCount - 1;
                    }

                    // Последний день отпуска
                    DateTime endDate = currentDate.AddDays(-1);


                    // Можно ли добавлять отпуск?
                    if (CheckVacation(AllVacations, VacationsEmployee, CurrentVacationEmployee, startDate, endDate) == true)
                    {
                        VacationsEmployee.AddRange(CurrentVacationEmployee);
                        AllVacations.AddRange(CurrentVacationEmployee);
                        allVacationCount = allVacationCount - difference;
                    }
                }
            }
            PrintVacations(VacationDictionary);
        }



        // Инициализация коллекции
        static Dictionary<string, List<DateTime>> Initialize()
        {
            Dictionary<string, List<DateTime>> VacationDictionary = new Dictionary<string, List<DateTime>>()
            {
                ["Иванов Иван Иванович"] = new List<DateTime>(),
                ["Петров Петр Петрович"] = new List<DateTime>(),
                ["Юлина Юлия Юлиановна"] = new List<DateTime>(),
                ["Сидоров Сидор Сидорович"] = new List<DateTime>(),
                ["Павлов Павел Павлович"] = new List<DateTime>(),
                ["Георгиев Георг Георгиевич"] = new List<DateTime>()
            };
            return VacationDictionary;
        }



        // Если выходной день, тогда в отпуске он не учитывается
        static void AddWeekendDay(DateTime date, ref int currentVacationCount)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                currentVacationCount = currentVacationCount + 1;
            }
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                currentVacationCount = currentVacationCount + 1;
            }
        }



        // Проверка отпуска на условие
        static bool CheckVacation(List<DateTime> AllVacations, List<DateTime> VacationsEmployee, List<DateTime> CurrentVacationEmployee, DateTime startDate, DateTime endDate)
        {
            // Существует ли отпуск с такой датой начала и конца
            bool existStart = false;
            bool existEnd = false;

            if (AllVacations.Any(element => element >= startDate && element <= endDate) != true)
            {
                // Интервал между отпусками всех сотрудников - 3 дня
                if (AllVacations.Any(element => element.AddDays(3) >= startDate && element.AddDays(3) <= endDate) != true)
                {
                    // Интервал между отпусками одного работника - 14 дней
                    existStart = VacationsEmployee.Any(element => element.AddDays(14) >= startDate && element.AddDays(14) >= endDate);
                    existEnd = VacationsEmployee.Any(element => element.AddDays(-14) <= startDate && element.AddDays(-14) <= endDate);

                    if (existStart == false || existEnd == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        // Вывод списка всех отпусков
        static void PrintVacations(Dictionary<string, List<DateTime>> VacationDictionary)
        {
            Console.WriteLine("Список отпусков всех сотрудников за 2023 год:");
            Console.WriteLine("--------------------------------------------- \r\n");

            foreach (var Vacation in VacationDictionary)
            {
                Console.WriteLine("- Сотрудник: " + Vacation.Key);
                Console.WriteLine("Дни отпуска вместе с выходными днями (всего дней: " + Vacation.Value.Count() + "):");

                foreach (var item in Vacation.Value)
                {
                    Console.WriteLine(item.ToString("d"));
                }

                Console.WriteLine("\r\n");
            }
        }



    }
}