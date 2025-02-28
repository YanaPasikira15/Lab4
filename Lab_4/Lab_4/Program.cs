﻿using System;
using System.Collections.Generic;
using System.Linq;

// Клас Товар
class Товар
{
    public string Назва { get; set; }
    public decimal Ціна { get; set; }
    public string Опис { get; set; }
    public string Категорія { get; set; }
    public double Рейтинг { get; set; }

    public Товар(string назва, decimal ціна, string опис, string категорія, double рейтинг)
    {
        Назва = назва;
        Ціна = ціна;
        Опис = опис;
        Категорія = категорія;
        Рейтинг = рейтинг;
    }

    public override string ToString()
    {
        return $"{Назва} - {Категорія} - {Ціна} грн - Рейтинг: {Рейтинг}";
    }
}

// Клас Користувач
class Користувач
{
    public string Логін { get; set; }
    public string Пароль { get; set; }
    public List<Замовлення> ІсторіяПокупок { get; set; }

    public Користувач(string логін, string пароль)
    {
        Логін = логін;
        Пароль = пароль;
        ІсторіяПокупок = new List<Замовлення>();
    }

    public void ДодатиЗамовлення(Замовлення замовлення)
    {
        ІсторіяПокупок.Add(замовлення);
    }
}

// Клас Замовлення
class Замовлення
{
    public List<Товар> Товари { get; set; }
    public List<int> Кількість { get; set; }
    public decimal ЗагальнаВартість { get; private set; }
    public string Статус { get; set; } 

    public Замовлення()
    {
        Товари = new List<Товар>();
        Кількість = new List<int>();
        Статус = "Обробляється";
    }

    public void ДодатиТовар(Товар товар, int кількість)
    {
        Товари.Add(товар);
        Кількість.Add(кількість);
        ЗагальнаВартість += товар.Ціна * кількість;
    }

    public override string ToString()
    {
        return $"Загальна вартість: {ЗагальнаВартість} грн - Статус: {Статус}";
    }
}

// Інтерфейс ISearchable для пошуку товарів
interface ISearchable
{
    List<Товар> ПошукЗаЦіною(decimal мінЦіна, decimal максЦіна);
    List<Товар> ПошукЗаКатегорією(string категорія);
    List<Товар> ПошукЗаРейтингом(double мінРейтинг);
}

// Клас Магазин
class Магазин : ISearchable
{
    private List<Товар> товари;
    private List<Користувач> користувачі;
    private List<Замовлення> замовлення;

    public Магазин()
    {
        товари = new List<Товар>();
        користувачі = new List<Користувач>();
        замовлення = new List<Замовлення>();
    }

    // Додавання товарів
    public void ДодатиТовар(Товар товар)
    {
        товари.Add(товар);
    }

    // Реалізація пошукових методів інтерфейсу ISearchable
    public List<Товар> ПошукЗаЦіною(decimal мінЦіна, decimal максЦіна)
    {
        return товари.Where(t => t.Ціна >= мінЦіна && t.Ціна <= максЦіна).ToList();
    }

    public List<Товар> ПошукЗаКатегорією(string категорія)
    {
        return товари.Where(t => t.Категорія.Equals(категорія, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Товар> ПошукЗаРейтингом(double мінРейтинг)
    {
        return товари.Where(t => t.Рейтинг >= мінРейтинг).ToList();
    }

    // Реєстрація та вхід користувача
    public void ЗареєструватиКористувача(string логін, string пароль)
    {
        if (користувачі.Any(k => k.Логін == логін))
            Console.WriteLine("Користувач з таким логіном вже існує.");
        else
            користувачі.Add(new Користувач(логін, пароль));
    }

    public Користувач Увійти(string логін, string пароль)
    {
        var користувач = користувачі.FirstOrDefault(k => k.Логін == логін && k.Пароль == пароль);
        if (користувач == null)
            Console.WriteLine("Неправильний логін або пароль.");
        return користувач;
    }

    // Оформлення замовлення
    public void ОформитиЗамовлення(Користувач користувач, Замовлення новеЗамовлення)
    {
        замовлення.Add(новеЗамовлення);
        користувач.ДодатиЗамовлення(новеЗамовлення);
    }

    // Виведення всіх товарів
    public void ПоказатиТовари()
    {
        foreach (var товар in товари)
            Console.WriteLine(товар);
    }
}

// Приклад використання
class Program
{
    static void Main(string[] args)
    {
        Магазин магазин = new Магазин();

        // Додавання товарів
        магазин.ДодатиТовар(new Товар("Телефон", 35000, "Смартфон з 1ТБ пам'яті", "Електроніка", 4.5));
        магазин.ДодатиТовар(new Товар("Диван", 30000, "Софа кутова лівостороння", "Меблі", 4.8));
        магазин.ДодатиТовар(new Товар("Кава", 100, "Мелена кава 250г", "Продукти", 4.3));

        // Реєстрація користувача
        магазин.ЗареєструватиКористувача("testuser", "password123");

        // Вхід користувача
        Користувач користувач = магазин.Увійти("testuser", "password123");
        if (користувач == null) return;

        // Пошук товарів за категорією
        Console.WriteLine("\nТовари в категорії 'Електроніка':");
        var електроніка = магазин.ПошукЗаКатегорією("Електроніка");
        електроніка.ForEach(Console.WriteLine);

        // Створення замовлення
        Замовлення замовлення = new Замовлення();
        замовлення.ДодатиТовар(електроніка[0], 1);
        магазин.ОформитиЗамовлення(користувач, замовлення);

        // Виведення історії покупок користувача
        Console.WriteLine("\nІсторія покупок:");
        користувач.ІсторіяПокупок.ForEach(Console.WriteLine);
    }
}
