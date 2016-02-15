using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlServerCe;
using System.Data;
using System.Windows.Forms;
using ResumesApp.Model;

namespace ResumesApp.DAL
{
    class InitDb
    {
        public static void InitDbFile()
        {
            string connectionString;
            string fileName = "Resumes.sdf";
            string password = "resumes";
            connectionString = string.Format("Datasource=\"{0}\"; Password=\"{1}\"", fileName, password);

            if (!File.Exists(fileName))
            {
                bool commit = CreateDB(connectionString);
                if (!commit && File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                else
                {
                    PopulateProfiles(GenerateProfiles(), connectionString);
                    PopulateRecentJobs(GenerateRecentJobs(GetProfilesIdBdate(connectionString)), connectionString);
                }
            }
        }

        static bool CreateDB(string connStr)
        {
            bool commit = false;
            SqlCeEngine en = new SqlCeEngine(connStr);
            try
            {
                en.CreateDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            SqlCeConnection cn = new SqlCeConnection(connStr);
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }

            List<SqlCeCommand> cmdList = new List<SqlCeCommand>();
            List<string> queries = new List<string>();
            queries.Add("CREATE TABLE [Profiles] (" +
                 "[Id] int IDENTITY (1,1) NOT NULL" +
                 ", [FullName] nvarchar(300) NOT NULL" +
                 ", [BirthDate] datetime NOT NULL" +
                 ", [BirthPlace] nvarchar(300) NOT NULL" +
                 ", [PassportData] nvarchar(300) NOT NULL" +
                 ", [PersonalQualities] nvarchar(300) NULL" +
                 ", [Characteristics] nvarchar(300) NULL" +
                 ", [EntryDate] datetime NOT NULL" +
                 ")");
            queries.Add("ALTER TABLE [Profiles] ADD CONSTRAINT [PK_Profiles] PRIMARY KEY ([Id])");
            queries.Add("CREATE TABLE [RecentJobs] (" +
                "[Id] int IDENTITY (1,1) NOT NULL" +
                ", [ProfileId] int NOT NULL" +
                ", [JobName] nvarchar(300) NOT NULL" +
                ", [ReceiptDate] datetime NOT NULL" +
                ", [DismissDate] datetime NULL" +
                ", [DismissReason] nvarchar(300) NULL" +
                ")");
            queries.Add("ALTER TABLE [RecentJobs] ADD CONSTRAINT [PK_RecentJobs] PRIMARY KEY ([Id])");
            queries.Add("ALTER TABLE [RecentJobs] ADD CONSTRAINT [FK_Profiles_RecentJobs] FOREIGN KEY ([ProfileId]) REFERENCES [Profiles]([Id])");

            foreach (string sql in queries)
            {
                cmdList.Add(new SqlCeCommand(sql, cn));
            }

            try
            {
                foreach (SqlCeCommand cmd in cmdList)
                {
                    cmd.ExecuteNonQuery();
                }
                commit = true;
            }
            catch (SqlCeException sqlexception)
            {
                MessageBox.Show(sqlexception.Message, "Sql Exception",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }

            return commit;
        }
        /*Profile section start*/
        static void PopulateProfiles(List<Profile> profiles, string connStr)
        {
            using (SqlCeConnection cn = new SqlCeConnection(connStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                foreach (Profile profile in profiles)
                {
                    InsertProfile(profile, cn);
                }

                cn.Close();
            }
        }
        static void InsertProfile(Profile profile, SqlCeConnection cn)
        {
            SqlCeCommand cmd;
            string sql = "insert into Profiles " +
                "(FullName, BirthDate, BirthPlace, PassportData, " +
                " PersonalQualities, Characteristics, EntryDate) " +
                "values (@FullName, @BirthDate, @BirthPlace, @PassportData," +
                " @PersonalQualities, @Characteristics, @EntryDate)";

            try
            {
                cmd = new SqlCeCommand(sql, cn);
                cmd.Parameters.AddWithValue("@FullName", profile.FullName);
                cmd.Parameters.AddWithValue("@BirthDate", profile.BirthDate);
                cmd.Parameters.AddWithValue("@BirthPlace", profile.BirthPlace);
                cmd.Parameters.AddWithValue("@PassportData", profile.PassportData);
                cmd.Parameters.AddWithValue("@PersonalQualities", profile.PersonalQualities);
                cmd.Parameters.AddWithValue("@Characteristics", profile.Characteristics);
                cmd.Parameters.AddWithValue("@EntryDate", profile.EntryDate);
                cmd.ExecuteNonQuery();
            }
            catch (SqlCeException sqlexception)
            {
                MessageBox.Show(sqlexception.Message, "Sql Exception"
                  , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception"
                  , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static List<Profile> GenerateProfiles()
        {
            int profsNum = 100;
            Random r = new Random();
            List<Profile> profiles = new List<Profile>();

            for (int i = 0; i < profsNum; i++)
            {
                Profile profile = new Profile();
                profile.FullName = GenerateFullname(r);
                profile.BirthDate = GenerateBirthDay(r);
                profile.BirthPlace = GenerateBirthPlace(r);
                profile.PassportData = GeneratePassportData(r, profile.BirthDate);
                profile.PersonalQualities = GeneratePersonalQualities(r);
                profile.Characteristics = GenerateCharacteristics(r);
                profile.EntryDate = GenerateEntryDate(r);

                profiles.Add(profile);
            }

            return profiles;
        }
        static string GenerateFullname(Random r)
        {
            string[] maleNames = new string[100] 
            { "Алан","Александр","Алексей","Альберт","Анатолий",
                "Андрей","Антон","Арсен","Арсений","Артем","Артемий",
                "Артур","Богдан","Борис","Вадим","Валентин","Валерий",
                "Василий","Виктор","Виталий","Владимир","Владислав",
                "Всеволод","Вячеслав","Геннадий","Георгий","Герман",
                "Глеб","Гордей","Григорий","Давид","Дамир","Даниил",
                "Демид","Демьян","Денис","Дмитрий","Евгений","Егор",
                "Елисей","Захар","Иван","Игнат","Игорь","Илья","Ильяс",
                "Камиль","Карим","Кирилл","Клим","Константин","Лев",
                "Леонид","Макар","Максим","Марат","Марк","Марсель",
                "Матвей","Мирон","Мирослав","Михаил","Назар","Никита",
                "Николай","Олег","Павел","Петр","Платон","Прохор",
                "Рамиль","Ратмир","Ринат","Роберт","Родион","Роман",
                "Ростислав","Руслан","Рустам","Савва","Савелий",
                "Святослав","Семен","Сергей","Станислав","Степан",
                "Тамерлан","Тимофей","Тимур","Тихон","Федор","Филипп",
                "Шамиль","Эдуард","Эльдар","Эмиль","Эрик","Юрий","Ян","Ярослав"};

            string[] malePatronymic = new string[100] 
            { "Ааронович", "Абрамович", "Авдеевич", "Аверьянович", "Адамович", 
                "Александрович", "Алексеевич", "Анатольевич", "Андреевич", 
                "Анисимович", "Антипович", "Антонович", "Арсенович", "Арсеньевич", 
                "Артёмович", "Артемьевич", "Артурович", "Архипович", "Афанасьевич", 
                "Бенедиктович", "Богданович", "Борисович", "Вадимович", "Валентинович",
                "Валерианович", "Валерьевич", "Валерьянович", "Васильевич", "Венедиктович", 
                "Вениаминович", "Викентьевич", "Викторович", "Витальевич", "Владиленович", 
                "Владимирович", "Владиславович", "Владленович", "Всеволодович", 
                "Вячеславович", "Гавриилович", "Гаврилович", "Геннадиевич", "Георгиевич", 
                "Герасимович", "Германович", "Гертрудович", "Глебович", "Гордеевич", 
                "Григорьевич", "Давыдович", "Даниилович", "Данилович", "Демидович", 
                "Демьянович", "Денисович", "Димитриевич", "Дмитриевич", "Дорофеевич", 
                "Егорович", "Елисеевич", "Еремеевич", "Ермилович", "Ермолаевич", 
                "Ерофеевич", "Ефимович", "Ефстафьевич", "Захарьевич", "Зиновьевич", 
                "Игнатьевич", "Игоревич", "Изотович", "Иларионович", "Ильич", "Иосифович", 
                "Исидорович", "Матвеевич", "Михайлович", "Петрович", "Семёнович", 
                "Сидорович", "Тарасович", "Терентьевич", "Тихонович", "Трифонович", 
                "Трофимович", "Фёдорович", "Федосеевич", "Федосьевич", "Федотович", 
                "Филатович", "Филимонович", "Филиппович", "Фомич", "Фролович", 
                "Харитонович", "Харламович", "Харлампович", "Эдуардович", 
                "Яковлевич", "Ярославович" };

            string[] maleSurnames = new string[100] 
            { "Иванов", "Васильев", "Петров", "Смирнов", "Михайлов", "Фёдоров", 
                "Соколов", "Яковлев", "Попов", "Андреев", "Алексеев", "Александров", 
                "Лебедев", "Григорьев", "Степанов", "Семёнов", "Павлов", "Богданов", 
                "Николаев", "Дмитриев", "Егоров", "Волков", "Кузнецов", "Никитин", 
                "Соловьёв", "Тимофеев", "Орлов", "Афанасьев", "Филиппов", "Сергеев", 
                "Захаров", "Матвеев", "Виноградов", "Кузьмин", "Максимов", "Козлов", 
                "Ильин", "Герасимов", "Марков", "Новиков", "Морозов", "Романов", 
                "Осипов", "Макаров", "Зайцев", "Беляев", "Гаврилов", "Антонов", 
                "Ефимов", "Леонтьев", "Давыдов", "Гусев", "Данилов", "Киселёв", 
                "Сорокин", "Тихомиров", "Крылов", "Никифоров", "Кондратьев", 
                "Кудрявцев", "Борисов", "Жуков", "Воробьёв", "Щербаков", "Поляков", 
                "Савельев", "Шмидт", "Трофимов", "Чистяков", "Баранов", "Сидоров", 
                "Соболев", "Карпов", "Белов", "Миллер", "Титов", "Львов", "Фролов", 
                "Игнатьев", "Комаров", "Прокофьев", "Быков", "Абрамов", "Голубев", 
                "Пономарёв", "Покровский", "Мартынов", "Кириллов", "Шульц", "Миронов", 
                "Фомин", "Власов", "Троицкий", "Федотов", "Назаров", "Ушаков", 
                "Денисов", "Константинов", "Воронин", "Наумов" };

            string[] femaleNames = new string[100] 
            { "Агата", "Агния", "Аделина", "Аида", "Аксинья", "Александра", 
                "Алена", "Алина", "Алиса", "Алия", "Алла", "Альбина", "Амелия", 
                "Амина", "Анастасия", "Ангелина", "Анна", "Антонина", "Ариана", 
                "Арина", "Валентина", "Валерия", "Варвара", "Василина", "Василиса", 
                "Вера", "Вероника", "Виктория", "Виолетта", "Владислава", "Галина", 
                "Дарина", "Дарья", "Диана", "Дина", "Ева", "Евангелина", "Евгения", 
                "Екатерина", "Елена", "Елизавета", "Есения", "Жанна", "Зарина", 
                "Злата", "Илона", "Инна", "Ирина", "Камилла", "Карина", "Каролина", 
                "Кира", "Клавдия", "Кристина", "Ксения", "Лариса", "Лейла", "Лиана", 
                "Лидия", "Лилия", "Лина", "Лия", "Любовь", "Людмила", "Майя", 
                "Маргарита", "Марианна", "Марина", "Мария", "Мелания", "Мила", 
                "Милана", "Милена", "Мирослава", "Надежда", "Наталья", "Нелли", 
                "Ника", "Нина", "Оксана", "Олеся", "Ольга", "Полина", "Регина", 
                "Сабина", "Светлана", "София", "Стефания", "Таисия", "Тамара", 
                "Татьяна", "Ульяна", "Эвелина", "Элина", "Эльвира", "Эльмира", 
                "Эмилия", "Юлия", "Яна", "Ярослава" };

            string[] femalePatronymic = new string[91] 
            { "Августовна", "Акимовна", "Александровна", "Алексеевна", "Антоновна", 
                "Андреевна", "Аркадьевна", "Батьковна", "Васильевна", "Вениаминовна", 
                "Викторовна", "Владимировна", "Глебовна", "Дмитриевна", "Евдокимовна", 
                "Ивановна", "Игоревна", "Константиновна", "Михайловна", "Николаевна", 
                "Олеговна", "Павловна", "Петровна", "Прохоровна", "Семёновна", 
                "Сергеевна", "Сидоровна", "Соломоновна", "Тимофеевна", "Фёдоровна", 
                "Юрьевна", "Яковлевна", "Архиповна", "Аскольдовна", "Альбертовна", 
                "Афанасьевна", "Анатольевна", "Артемовна", "Богдановна", "Болеславовна", 
                "Борисовна", "Вадимовна", "Валентиновна", "Владиславовна", "Валериевна", 
                "Вячеславовна", "Геннадиевна", "Георгиевна", "Геннадьевна", "Григорьевна", 
                "Даниловна", "Евгеньевна", "Егоровны", "Егоровна", "Ефимовна", "Ждановна", 
                "Захаровна", "Ильинична", "Кирилловна", "Кузминична", "Кузьминична", 
                "Леонидовна", "Леоновна", "Львовна", "Макаровна", "Матвеевна", 
                "Максимовна", "Мироновна", "Натановна", "Никифоровна", "Ниловна", 
                "Наумовна", "Оскаровна", "Робертовна", "Рубеновна", "Руслановна", 
                "Романовна", "Рудольфовна", "Святославовна", "Степановна", "Семеновна", 
                "Станиславовна", "Тарасовна", "Тимуровна", "Федоровна", "Феликсовна", 
                "Филипповна", "Харитоновна", "Эдуардовна", "Эльдаровна", "Юльевна" };

            string[] femaleSurnames = new string[100] 
            { "Ковалёва", "Ильина", "Гусева", "Титова", "Кузьмина", "Кудрявцева", 
                "Баранова", "Куликова", "Алексеева", "Степанова", "Яковалева", "Сорокина",
                "Сергеева", "Романова", "Захарова", "Борисова", "Королева", "Герасимова", 
                "Пономарева", "Григорьева", "Лазарева", "Медведева", "Ершова", "Никитина", 
                "Соболева", "Рябова", "Полякова", "Цветкова", "Данилова", "Жукова", 
                "Фролова", "Журавлева", "Николаева", "Путина", "Молчанова", "Крылова", 
                "Максимова", "Сидорова", "Осипова", "Белоусова", "Федотова", "Дорофеева", 
                "Егорова", "Панина", "Матвеева", "Боброва", "Дмитриева", "Калинина", 
                "Анисимова", "Петухова", "Пугачева", "Антонова", "Тимофеева", "Никифорова", 
                "Веселова", "Филиппова", "Маркова", "Большакова", "Суханова", "Миронова",
                "Александрова", "Коновалова", "Шестакова", "Казакова", "Ефимова", "Денисова", 
                "Громова", "Фомина", "Андреева", "Давыдова", "Мельникова", "Щербакова",
                "Блинова", "Колесникова", "Иванова", "Смирнова", "Кузнецова", "Попова", 
                "Соколова", "Лебедева", "Козлова", "Новикова", "Морозова", "Петрова", 
                "Волкова", "Соловаьева", "Васильева", "Зайцева", "Павлова", "Семенова",
                "Голубева", "Виноградова", "Богданова", "Воробьева", "Федорова", "Михайлова", 
                "Беляева", "Тарасова", "Белова", "Комарова" };

            //0 - female
            //1 - male
            if (r.Next(0, 2) == 0)
            {
                return femaleSurnames[r.Next(0, femaleSurnames.Length - 1)] + " " +
                    femaleNames[r.Next(0, femaleNames.Length - 1)] + " " +
                    femalePatronymic[r.Next(0, femalePatronymic.Length - 1)]
                    ;
            }
            else
            {
                return maleSurnames[r.Next(0, maleSurnames.Length - 1)] + " " +
                    maleNames[r.Next(0, maleNames.Length - 1)] + " " +
                    malePatronymic[r.Next(0, malePatronymic.Length - 1)]
                    ;
            }
        }
        static DateTime GenerateBirthDay(Random r)
        {
            DateTime start = DateTime.Today.AddYears(-60);
            int range = (DateTime.Today.AddYears(-18) - start).Days;
            return start.AddDays(r.Next(range));
        }
        static string GenerateBirthPlace(Random r)
        {
            string[] cities = new string[100] 
            { "г. Москва", "г. Санкт-Петербург", "г. Новосибирск", "г. Екатеринбург", 
                "г. Нижний Новгород", "г. Казань", "г. Челябинск", "г. Омск", 
                "г. Самара", "г. Ростов-на-Дону", "г. Уфа", "г. Красноярск", 
                "г. Пермь", "г. Воронеж", "г. Волгоград", "г. Саратов", "г. Краснодар", 
                "г. Тольятти", "г. Тюмень", "г. Ижевск", "г. Барнаул", "г. Иркутск", 
                "г. Ульяновск", "г. Хабаровск", "г. Владивосток", "г. Ярославль", 
                "г. Махачкала", "г. Томск", "г. Оренбург", "г. Новокузнецк", 
                "г. Кемерово", "г. Рязань", "г. Астрахань", "г. Набережные Челны", 
                "г. Пенза", "г. Липецк", "г. Киров", "г. Тула", "г. Чебоксары", 
                "г. Калининград", "г. Курск", "г. Улан - Удэ", "г. Ставрополь", 
                "г. Магнитогорск", "г. Тверь", "г. Иваново", "г. Брянск", 
                "г. Севастополь", "г. Сочи", "г. Белгород", "г. Нижний Тагил", 
                "г. Владимир", "г. Архангельск", "г. Калуга", "г. Сургут", 
                "г. Чита", "г. Симферополь", "г. Смоленск", "г. Волжский", 
                "г. Курган", "г. Орёл", "г. Череповец", "г. Вологда", 
                "г. Владикавказ", "г. Мурманск", "г. Саранск", "г. Якутск", 
                "г. Тамбов", "г. Грозный", "г. Стерлитамак", "г. Кострома", 
                "г. Петрозаводск", "г. Нижневартовск", "г. Йошкар-Ола", 
                "г. Новороссийск", "г. Балашиха", "г. Таганрог", 
                "г. Комсомольск-на-Амуре", "г. Сыктывкар", "г. Нальчик", 
                "г. Шахты", "г. Братск", "г. Нижнекамск", "г. Дзержинск", 
                "г. Орск", "г. Химки", "г. Ангарск", "г. Благовещенск", 
                "г. Подольск", "г. Великий Новгород", "г. Энгельс", "г. Старый Оскол", 
                "г. Королёв ", "г. Псков", "г. Бийск", "г. Прокопьевск", 
                "г. Балаково", "г. Рыбинск", "г. Южно-Сахалинск", "г. Армавир" };

            return cities[r.Next(cities.Length - 1)];
        }
        static string GeneratePassportData(Random r, DateTime bDate)
        {
            DateTime deliveryDate;
            int age = (DateTime.Today - bDate).Days / 365;
            if (age <= 20)
            {
                DateTime start = bDate.AddYears(18);
                int range = (DateTime.Today - start).Days;
                deliveryDate = start.AddDays(r.Next(range));
            }
            else if (age <= 45)
            {
                DateTime start = bDate.AddYears(20);
                int range = (DateTime.Today - start).Days;
                deliveryDate = start.AddDays(r.Next(range));
            }
            else
            {
                DateTime start = bDate.AddYears(45);
                int range = (DateTime.Today - start).Days;
                deliveryDate = start.AddDays(r.Next(range));
            }

            return "Паспорт серия " + r.Next(9999) + " номер " + r.Next(999999) +
                " выдан отделом УФМС России в " + GenerateBirthPlace(r) + ", " +
                "дата выдачи " + deliveryDate.ToString("d");
        }
        static string GeneratePersonalQualities(Random r)
        {
            int qualCount = 5;
            string[] pq = new string[89] 
            { "Адекватная самооценка", "аккуратность", "активность", "амбициозность", 
                "аналитический склад ума ", "Вежливость", "владеющий собой", 
                "внимательность к деталям", "внимательность к людям", "Гибкость", 
                "грамотность", "Дипломированность", "дисциплинированность", 
                "добросовестность", "добропорядочность", "здравомыслие", 
                "Изобретательность", "индивидуалистичность", "инициативность", 
                "интеллектуальность", "исполнительность", "Квалифицированность", 
                "коммуникабельность", "компетентность", "креативность", 
                "Легкообучаемость", "лидерство", "Мобильность", "Надежность", 
                "наличие организаторских способностей", "находчивость", 
                "независимость", "Обучаемость (легко)", "общительность", 
                "обязательность", "оптимистичность", "организованность", 
                "ориентированность на достижение результата", "осмотрительность", 
                "ответственность", "отзывчивость", "Память (отличная)", 
                "подготовленность", "практичность", "преданность", 
                "предприимчивость", "предусмотрительность", "прилежность", 
                "приятность", "прозорливость", "проницательность", "просвещенность", 
                "профессиональность", "прямолинейность", "пунктуальность", 
                "Работоспособность", "разносторонняя развитость", "рациональность", 
                "реалистичность", "решительность", "Самокритичность", "самостоятельность", 
                "серьезность", "склонность к соперничеству", "склонность к подвижной работе", 
                "способность работать в команде", "справедливость", "старательность", 
                "Тактичность", "талантливость", "творческий подход", "терпеливость", 
                "Убедительность", "уверенность в себе", "увлеченность", 
                "умение работать в команде", "умение работать самостоятельно", 
                "упорность", "уравновешенность", "усердность", "Целеустремленность", 
                "Честность", "умение выражать свои мысли", "чувство собственного достоинства", 
                "Широких взглядов", "Эмоциональность", "энергичность", 
                "энтузиазм", "Яркость" };

            var numbers = Enumerable.Range(1, pq.Length - 1).OrderBy(x => r.Next()).Take(qualCount).ToList();
            StringBuilder str = new StringBuilder();
            foreach (var n in numbers)
            {
                str.Append(pq[n]);
                if (n != numbers[numbers.Count - 1]) str.Append(", ");
            }
            return str.ToString();
        }
        static string GenerateCharacteristics(Random r)
        {
            int qualCount = 5;
            string[] characters = new string[58] 
            { "активный", "быстро адаптирующийся", "амбициозный", 
                "с широкими взглядами", " интересами", "неунывающий", 
                " жизнерадостный", "способный соревноваться", " конкурентоспособный", 
                "открытый к сотрудничеству", "творческий", "любопытный", "решительный", 
                "устремленный", "коммуникабельный", "энергичный", "полный энтузиазма", 
                " энергии", "предприимчивый (то есть", " способный реализовывать идеи)", 
                "психологически гибкий", "дружелюбный", "щедрый", "приятный", 
                "способный много работать", " трудолюбивый", "полезный", "честный", 
                "имеющий богатое воображение", "независимый", "трудолюбивый", 
                "усердный", "интеллектуальный", "лидер", "психологически зрелый", 
                "с четкой мотивацией", "оптимистичный", "организованны", "оригинальный", 
                "человек с легким", " открытым характером", "терпеливый", "прогрессивный", 
                "целеустремленный", "быстрый", "надежный", "изобретательный", 
                "находчивый", "уверенный в себе", "Самостоятельный", 
                "самодостаточный", "Серьезный", "коммуникабельный", "успешный", 
                "готовый предоставить поддержку", "тактичный", "добросовестный", 
                "заслуживающий доверия" };

            var numbers = Enumerable.Range(1, characters.Length - 1).OrderBy(x => r.Next()).Take(qualCount).ToList();
            StringBuilder str = new StringBuilder();
            foreach (var n in numbers)
            {
                str.Append(characters[n]);
                if (n != numbers[numbers.Count - 1]) str.Append(", ");
            }
            return str.ToString();
        }
        static DateTime GenerateEntryDate(Random r)
        {
            DateTime start = DateTime.Today.AddYears(-1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(r.Next(range));
        }
        /*Profile section end*/

        /*RecentJob section start*/
        static void PopulateRecentJobs(List<RecentJob> recentJobs, string connStr)
        {
            using (SqlCeConnection cn = new SqlCeConnection(connStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                foreach (RecentJob recentJob in recentJobs)
                {
                    InsertRecentJob(recentJob, cn);
                }

                cn.Close();
            }
        }
        static void InsertRecentJob(RecentJob recentJob, SqlCeConnection cn)
        {
            SqlCeCommand cmd;
            string sql = "insert into RecentJobs " +
                "(ProfileId, JobName, ReceiptDate, DismissDate, " +
                " DismissReason) " +
                "values (@ProfileId, @JobName, @ReceiptDate, @DismissDate," +
                " @DismissReason)";

            try
            {
                cmd = new SqlCeCommand(sql, cn);
                cmd.Parameters.AddWithValue("@ProfileId", recentJob.ProfileId);
                cmd.Parameters.AddWithValue("@JobName", recentJob.JobName);
                cmd.Parameters.AddWithValue("@ReceiptDate", recentJob.ReceiptDate);
                if (recentJob.DismissDate != DateTime.MinValue)
                {
                    cmd.Parameters.AddWithValue("@DismissDate", recentJob.DismissDate);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DismissDate", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@DismissReason", recentJob.DismissReason ?? Convert.DBNull);
                cmd.ExecuteNonQuery();
            }
            catch (SqlCeException sqlexception)
            {
                MessageBox.Show(sqlexception.Message, "Sql Exception"
                  , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception"
                  , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static List<Profile> GetProfilesIdBdate(string connStr)
        {
            List<Profile> profiles = new List<Profile>();
            using (SqlCeConnection cn = new SqlCeConnection(connStr))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                SqlCeCommand cmd;
                SqlCeDataReader reader;
                string sql = "SELECT Id, BirthDate FROM Profiles";
                try
                {
                    cmd = new SqlCeCommand(sql, cn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        profiles.Add(new Profile() { Id = reader.GetInt32(0), BirthDate = reader.GetDateTime(1) });
                    }
                    reader.Close();
                }
                catch (SqlCeException sqlexception)
                {
                    MessageBox.Show(sqlexception.Message, "Sql Exception"
                      , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception"
                      , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cn.Close();
            }
            return profiles;
        }
        static List<RecentJob> GenerateRecentJobs(List<Profile> profiles)
        {
            Random r = new Random();
            List<RecentJob> recentJobs = new List<RecentJob>();

            foreach (var profile in profiles)
            {
                DateTime expStart = GetExpStart(r, profile.BirthDate);
                if ((DateTime.Today - expStart).Days < 100)
                {
                    recentJobs.Add(new RecentJob()
                    {
                        ProfileId = profile.Id,
                        ReceiptDate = expStart,
                        JobName = GetJobName(r)
                    });
                }
                else
                {
                    bool isEnd = false;
                    DateTime receiptDate = expStart;
                    DateTime dismissDate;
                    while (!isEnd)
                    {
                        dismissDate = receiptDate.AddDays(r.Next(100, (DateTime.Today - receiptDate).Days));
                        if ((DateTime.Today - dismissDate).Days < 100)
                        {
                            recentJobs.Add(new RecentJob()
                            {
                                ProfileId = profile.Id,
                                ReceiptDate = receiptDate,
                                JobName = GetJobName(r)
                            });
                            isEnd = true;
                        }
                        else
                        {
                            recentJobs.Add(new RecentJob()
                            {
                                ProfileId = profile.Id,
                                ReceiptDate = receiptDate,
                                JobName = GetJobName(r),
                                DismissDate = dismissDate,
                                DismissReason = getDismissReason(r)
                            });
                            receiptDate = dismissDate;
                        }
                    }
                }
            }

            return recentJobs;
        }
        static DateTime GetExpStart(Random r, DateTime bDate)
        {
            //Получение даты начала стажа
            DateTime start;
            if ((DateTime.Today - bDate).Days / 365 > 23)
            {
                start = bDate.AddYears(r.Next(18, 23));
            }
            else
            {
                start = bDate.AddYears(18);
            }

            return start;
        }
        static string GetJobName(Random r)
        {
            string[] orgNames = new string[100] 
            {"Егоров, Пугинский, Афанасьев и партнеры","Пепеляев Групп",
                "DLA Piper","Salans","Baker & McKenzie","Magisters",
                "Вегас-Лекс","Allen & Overy","Noerr","Юков, Хренов и партнеры",
                "Яковлев и партнеры","CMS Legal","Sameta","Linklaters","ЮСТ",
                "Hannes Snellman","Муранов, Черняков и партнеры","Macleod Dixon",
                "Качкин и партнеры","Chadbourne & Parke","AstapovLawyers",
                "Capital Legal Services","Налоговик","Юстина",
                "Skadden, Arps, Slate, Meagher & Flom",
                "Юридическое бюро Падва и Эпштейн","Debevoise & Plimpton",
                "Юринфлот","ЮК Проф","Каменская и партнеры","ИНТЕЛЛЕКТ-С",
                "Duvernoix Legal","Правовой центр «Интеллект»","JBI Эксперт",
                "Бизнес и право","Корельский, Ищук, Астафьев и партнеры",
                "Коблев и партнеры","Патентно-правовая фирма ЮС",
                "Юридическая группа Ратум","Юридическое бюро ПокровЪ",
                "Клифф","Адвокатсткое бюро «Юг»","Бартолиус","Юридическое бюро Юрьева",
                "Партнерское бюро «АйТи-Каунсел»","Городисский и партнеры",
                "Левант и партнеры","ЮниЛекс","Тимофеев, Фаренвальд и партнеры",
                "Юрэнерго","S&K Вертикаль","Институт проблем предпринимательства",
                "Линия права","ЮНЭКС","ЮРВЕСТ",
                "Башкирская специализированная коллегия адвокатов",
                "Первая Юридическая Компания","Юридическая компания АдвокатЪ",
                "Андрей Городисский и партнеры","Сашенькин и Райт","Юрико",
                "Адвокатский кабинет Дениса Шашкина","Михайленко, Сокол и партнеры",
                "Юридический центр «Де-Конс»","Алимирзоев и Трофимов","Райтмарк груп",
                "Некоммерческое партнерство «Правоведы»","Плешаков, Ушкалов и партнеры",
                "СтройКапиталКонсалтинг","ЮАП-СПб","Региональная правовая компания",
                "Агентство правовых технологий «Магистр»","Мюллер и Аверин","Тюмень Юристикс",
                "Адвокат ФРЕММ","Барабашев и партнеры","Уфа-Адвокат Vindex Group","Юс Ауреум",
                "Когитум","Консалт","Топ Консалт","ПравоГрад","Олевинский, Буюкян и партнеры",
                "Эберг, Степанов и партнеры","Дмитрий Матвеев и партнеры",
                "Центр правового обслуживания","Суррей","Мейер, Яковлев и партнеры",
                "ЭНСО","Юридическая фирма ПАРТНЕР","Городская коллегия адвокатов",
                "Бест Консалтинг Групп","Екатеринбургская Правовая Компания",
                "Юридическое агентство Фемида","Право и Консультации",
                "Salomon Partners","КОНСУЛЪ","КА Магаданской области Дальневосточная",
                "ЛексФинанс Груп","Арман" };

            return orgNames[r.Next(orgNames.Length - 1)];
        }
        static string getDismissReason(Random r)
        {
            string[] reasons = new string[10] 
            { "Низкая заработная плата", "Отсутствие перспектив роста", 
                "Конфликты с руководством", "Невозможность саморазвития", 
                "Неинтересные задачи", "Нерегулярная зарплата", "Атмосфера в коллективе", 
                "Зарплата в конвертах", "Несвобода в принятии решений", 
                "Жесткий график работы" };

            return reasons[r.Next(reasons.Length - 1)];
        }
        /*RecentJob section end*/
    }
}