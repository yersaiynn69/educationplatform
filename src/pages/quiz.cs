using System;
using System.Collections.Generic;
using System.Linq;

public class Question
{
    public string Text { get; set; }
    public string[] Options { get; set; }
    public HashSet<int> CorrectAnswers { get; set; }

    public Question(string text, string[] options, HashSet<int> correctAnswers)
    {
        Text = text;
        Options = options;
        CorrectAnswers = correctAnswers;
    }
}

public class Program
{
    private static List<Question> allQuestions = new List<Question>();
    private static List<Question> selectedQuestions = new List<Question>();
    private static int correctAnswersCount = 0;
    private static string userName = string.Empty;
    private static string selectedSubject = string.Empty;

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("===== Викторина =====");

        Console.Write("Аты-жөніңізді енгізіңіз: ");
        userName = Console.ReadLine();

        Console.WriteLine("\nПән таңдаңыз:");
        string[] subjects = { "C# тілі", "Java", "Қазақ тілі" };
        for (int i = 0; i < subjects.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {subjects[i]}");
        }

        int subjectChoice;
        while (!int.TryParse(Console.ReadLine(), out subjectChoice) || subjectChoice < 1 || subjectChoice > subjects.Length)
        {
            Console.Write("Қате таңдау. Қайтадан таңдаңыз: ");
        }

        selectedSubject = subjects[subjectChoice - 1];

        LoadQuestions();
        selectedQuestions = allQuestions.OrderBy(q => Guid.NewGuid()).Take(10).ToList();

        for (int i = 0; i < selectedQuestions.Count; i++)
        {
            Console.Clear();
            var question = selectedQuestions[i];

            Console.WriteLine($"Сұрақ {i + 1}: {question.Text}\n");

            for (int j = 0; j < question.Options.Length; j++)
            {
                Console.WriteLine($"{j + 1}. {question.Options[j]}");
            }

            Console.Write("\nЖауап нөмір(лері)н енгізіңіз (бірнешеуін пробелмен бөліңіз): ");
            var input = Console.ReadLine();
            var selectedOptions = new HashSet<int>();

            if (!string.IsNullOrWhiteSpace(input))
            {
                foreach (var part in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(part, out int index))
                    {
                        selectedOptions.Add(index - 1);
                    }
                }
            }

            if (selectedOptions.SetEquals(question.CorrectAnswers))
            {
                correctAnswersCount++;
            }
        }

        ShowResults();
    }

    private static void ShowResults()
    {
        int totalQuestions = selectedQuestions.Count;
        float scorePercentage = (correctAnswersCount * 100f) / totalQuestions;
        int finalGrade = scorePercentage switch
        {
            >= 90 => 5,
            >= 75 => 4,
            >= 50 => 3,
            _ => 2
        };

        Console.WriteLine("\n===== Нәтиже =====");
        Console.WriteLine($"Қатысушы: {userName}");
        Console.WriteLine($"Таңдалған пән: {selectedSubject}");
        Console.WriteLine($"Дұрыс жауаптар: {correctAnswersCount} / {totalQuestions}");
        Console.WriteLine($"Жауап пайызы: {scorePercentage:F2}%");
        Console.WriteLine($"Баға: {finalGrade}");
    }

    private static void LoadQuestions()
    {
        allQuestions.Clear();

        if (selectedSubject == "C# тілі")
        {
            LoadCSharpQuestions();
        }
        else if (selectedSubject == "Java")
        {
            LoadJavaQuestions();
        }
        else if (selectedSubject == "Қазақ тілі")
        {
            LoadKazakhLanguageQuestions();
        }
    }

    private static void LoadCSharpQuestions()
    {
        allQuestions.Add(new Question(
            "C# тілінде бүтін сан қалай жарияланады?",
            new[] { "char x;", "long x;", "int x;", "float x;", "bool x;", "var x;" },
            new HashSet<int> { 2, 5 }));

        allQuestions.Add(new Question(
            "C# тілінде консольге шығару үшін не қолданылады?",
            new[] { "System.out.println()", "scanf()", "Console.WriteLine()", "echo()", "print()", "cout <<" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "C# тілінде шартты оператор қалай жазылады?",
            new[] { "switch (x)", "if x > 0 then", "if (x > 0)", "x > 0 ?", "for (x > 0)", "while (x > 0)" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "C# тілінде массивті қалай жариялайды?",
            new[] { "int[] arr = new int[5];", "List arr = new List();", "int arr[5];", "array arr = int[5];", "int arr = new int[];", "int arr = {1,2,3};" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "C# тілінде foreach циклі қалай жазылады?",
            new[] { "loop x in arr", "foreach (int x in arr)", "for (int x : arr)", "repeat x in arr", "each (x in arr)", "for x in arr" },
            new HashSet<int> { 1 }));

        allQuestions.Add(new Question(
            "C# тілінде логикалық операторлар қалай жазылады?",
            new[] { "&&&, |||, !!", "xor, and, not", "&&, ||, !", "&, |, ~", "plus, minus, not", "and, or, not" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "C# тілінде класс қалай жарияланады?",
            new[] { "new class MyClass()", "Class MyClass { }", "define class MyClass { }", "class MyClass { }", "class = MyClass { }", "type MyClass {}" },
            new HashSet<int> { 3 }));

        allQuestions.Add(new Question(
            "C# тілінде әдіс (метод) қалай анықталады?",
            new[] { "fun MyMethod(): void {}", "function MyMethod()", "void MyMethod() { }", "MyMethod() -> void {}", "def MyMethod()", "method MyMethod() { }" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "int.Parse(\"123\") не үшін қолданылады?",
            new[] { "Қатені өңдейді", "Шартты оператор орындайды", "Мәтінді бүтін санға түрлендіреді", "Айнымалы жояды", "Массив жасайды", "Бүтін санды мәтінге түрлендіреді" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "try-catch блогының мақсаты қандай?",
            new[] { "Цикл құру", "Айнымалыны жариялау", "Қате өңдеу", "Дерекқорға қосылу", "Метод жазу", "Массивті жариялау" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "C# тілінде интерфейс қалай жазылады?",
            new[] { "Interface IMyInterface { }", "interface = MyInterface {}", "class interface MyClass {}", "define interface()", "interface IMyInterface { }", "using interface()" },
            new HashSet<int> { 4 }));

        allQuestions.Add(new Question(
            "C# тіліндегі using кілт сөзі не үшін қажет?",
            new[] { "Операцияны қайталау үшін", "Массив ашу үшін", "Аты кеңістігін қосу үшін", "Шарт тексеру үшін", "Цикл бастау үшін", "Айнымалы жариялау үшін" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "C# тіліндегі List<int> дегеніміз не?",
            new[] { "Қате өңдеу құралы", "Метод", "Бүтін сан", "Интерфейс", "Динамикалық бүтін сандар тізімі", "Статикалық массив" },
            new HashSet<int> { 4 }));

        allQuestions.Add(new Question(
            "== мен = айырмашылығы неде?",
            new[] { "== тек string үшін", "= тек class үшін", "екеуі де тек int үшін", "== салыстыру, = меншіктеу", "екеуі де бірдей", "== тек if үшін" },
            new HashSet<int> { 3 }));

        allQuestions.Add(new Question(
            "C# тіліндегі enum не үшін қолданылады?",
            new[] { "Метод жасау үшін", "Массив жасау үшін", "Айнымалы жою үшін", "Қате өңдеу үшін", "Циклді жазу үшін", "Тұрақты мәндер жинағын анықтау үшін" },
            new HashSet<int> { 5 }));
    }

    private static void LoadJavaQuestions()
    {
        allQuestions.Add(new Question(
            "Java тілінде әдіс (метод) қалай жазылады?",
            new[] { "method MyMethod() {}", "void MyMethod() {}", "function MyMethod() {}", "fun MyMethod() {}", "def MyMethod()", "procedure MyMethod()" },
            new HashSet<int> { 1 }));

        allQuestions.Add(new Question(
            "Java тілінде консольге мәтін шығару үшін не қолданылады?",
            new[] { "System.out.print()", "Console.WriteLine()", "printf()", "cout <<", "println()", "echo()" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "Java тілінде класс қалай жарияланады?",
            new[] { "type MyClass {}", "define MyClass {}", "class MyClass {}", "Class MyClass() {}", "MyClass class {}", "object MyClass {}" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "Java тілінде массив қалай жарияланады?",
            new[] { "int arr[] = new int[5];", "List<int> arr = new List<>();", "array arr = int[5];", "int arr = new int[5];", "int arr = {1,2,3};", "var arr = [];" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "Java тілінде if шартты оператор қалай жазылады?",
            new[] { "if (x > 0)", "if x > 0 then", "if x > 0:", "when (x > 0)", "if x > 0 {}", "case (x > 0)" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "Java тілінде цикл құру үшін қандай циклдар бар?",
            new[] { "for", "foreach", "loop", "while", "repeat", "do-while" },
            new HashSet<int> { 0, 1, 3, 5 }));

        allQuestions.Add(new Question(
            "Java тілінде Scanner не үшін қолданылады?",
            new[] { "Файл жазу", "Код компиляциялау", "Деректерді консольден оқу", "Массив жариялау", "Шарт тексеру", "Айнымалыны көшіру" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "Java тілінде String дегеніміз не?",
            new[] { "Сан", "Бүтін сан", "Мәтін жолы", "Айнымалы атауы", "Метод", "Интерфейс" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "Java тілінде public дегеніміз не?",
            new[] { "Деректерді жасыру", "Айнымалы жою", "Әдістің немесе класстың қолжетімділігін ашық ету", "Цикл бастау", "Массив құру", "Қате өңдеу" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "Java тілінде try-catch блогының мақсаты қандай?",
            new[] { "Қате өңдеу", "Метод құру", "Циклді қайталау", "Айнымалы жариялау", "Массив жасау", "Функция жазу" },
            new HashSet<int> { 0 }));
    }

    private static void LoadKazakhLanguageQuestions()
    {
        allQuestions.Add(new Question(
            "Қазақ тілінде етістік қандай сөз табы?",
            new[] { "Зат есім", "Сын есім", "Сан есім", "Етістік", "Үстеу", "Шылау" },
            new HashSet<int> { 3 }));

        allQuestions.Add(new Question(
            "Қазақ тіліндегі тәуелдік жалғауларды көрсетіңіз.",
            new[] { "-мын, -мін", "-ның, -нің", "-лар, -лер", "-ым, -ім, -сы, -сі", "-бын, -бін", "-ды, -ді" },
            new HashSet<int> { 3 }));

        allQuestions.Add(new Question(
            "«Мектепке» сөзінде қандай жалғау бар?",
            new[] { "Септік жалғау", "Көптік жалғау", "Тәуелдік жалғау", "Жіктік жалғау", "Етістік жалғауы", "Шылау" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "Қазақ тіліндегі септіктер саны қанша?",
            new[] { "4", "5", "6", "7", "8", "9" },
            new HashSet<int> { 3 }));

        allQuestions.Add(new Question(
            "Қайсысы бастауыш қызметін атқарады?",
            new[] { "Жүгіріп барады", "Мектеп", "Кітапты оқыды", "Әдемі", "Жақсы көреді", "Оқыды" },
            new HashSet<int> { 1 }));

        allQuestions.Add(new Question(
            "Қазақ тілінде көптік жалғауларды табыңыз.",
            new[] { "-лар, -лер", "-дың, -дің", "-ға, -ге", "-ды, -ді", "-мен, -пен", "-мыз, -міз" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "Қазақ тілінде етістік қандай мағына береді?",
            new[] { "Затты білдіреді", "Сын-сапаны білдіреді", "Іс-әрекетті білдіреді", "Санды білдіреді", "Мекенді білдіреді", "Уақытты білдіреді" },
            new HashSet<int> { 2 }));

        allQuestions.Add(new Question(
            "Төмендегілердің қайсысы шылау сөздерге жатады?",
            new[] { "бірақ", "өте", "кеше", "көп", "үлкен", "оқу" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "Сөз таптарын атаңыз.",
            new[] { "Зат есім, етістік, сын есім", "Одағай, көмекші етістік", "Буын, бунақ", "Сөйлем мүшелері", "Көптік, септік жалғау", "Етістік, бастауыш" },
            new HashSet<int> { 0 }));

        allQuestions.Add(new Question(
            "Қайсысы сөйлем мүшесі бола алады?",
            new[] { "Буын", "Жалғау", "Бастауыш", "Жіктік жалғау", "Септік жалғау", "Шылау" },
            new HashSet<int> { 2 }));
    }


}
