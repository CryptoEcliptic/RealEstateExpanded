﻿using System;
using System.Collections.Generic;

namespace HomeHunter.Common
{
    public static class GlobalConstants
    {
        public const string AdministratorRoleName = "Admin";
        public const string UserRoleName = "User";
        public const string CompanyName = "Имотите ООД";
        public const string CompanyWebSite = "https://kkimotite.com";
        public const int UtcTimeCompensationZone = 2;
        public const string DateTimeVisualizationFormat = "dd-MM-yyyy HH:mm";
        public const string DateTimeGuestVisualizationFormat = "dd-MM-yyyy";
        public static int DefaultRealEstateYear = DateTime.Now.Year;
        public const string OfferTypeSaleName = "Продажба";
        public const string OfferTypeRentName = "Наем";
        public const int ImageUploadLimit = 20;
        public const string DefaultDateTimeDbValue = "01-01-0001 00:00";
        public const string NotAvailableMessage= "n/a";
        public const string PhoneValidationRegex = @"^([0-9]{10})$|^\+[0-9]{3}[0-9]{9}$|^\+[0-9]{3} [0-9]{3} [0-9]{3} [0-9]{3}$|^\+[0-9]{3} [0-9]{9}$|^[0-9]{4} [0-9]{3} [0-9]{3}$";
        public const string AllowedFileExtensionsAsString = ".jpg, .jpeg, .png, .bmp, .gif, .JPG";
        public const string BoolTrueStringValue = "ДА";
        public const string BoolFalseStringValue = "НЕ";

        public static List<string> AllowedFileExtensions = new List<string>
        {
            ".jpg", ".jpeg", ".png", ".bmp", ".gif"
        };

        public static List<string> ImotBgAppartmentTypes = new List<string>
        {
            "1-СТАЕН",
            "2-СТАЕН",
            "3-СТАЕН",
            "4-СТАЕН",
            "МНОГОСТАЕН",
            "МЕЗОНЕТ",
            "АТЕЛИЕ"
        };

        public static List<string> ImotBgBuildingTypes = new List<string>
        {
            "Тухла",
            "Панел",
            "ЕПК",
            "ПК",
            "Гредоред",
        };

        public static List<string> ImotBgSofiaDistricts = new List<string>
        {
            "град София, ",
            "град София, Банишора",
            "град София, Белите брези",
            "град София, Бенковски",
            "град София, Борово",
            "град София, Ботунец",
            "град София, Бояна",
            "град София, Бъкстон",
            "град София, Витоша",
            "град София, Военна рампа",
            "град София, Враждебна",
            "град София, Връбница 1",
            "град София, Връбница 2",
            "град София, Гевгелийски",
            "град София, Гео Милев",
            "град София, Горна баня",
            "град София, Горубляне",
            "град София, Гоце Делчев",
            "град София, Градина",
            "град София, Дианабад",
            "град София, Димитър Миленков",
            "град София, Докторски паметник",
            "град София, Драгалевци",
            "град София, Дружба 1",
            "град София, Дружба 2",
            "град София, Дървеница",
            "град София, ж.гр.Южен парк",
            "град София, Западен парк",
            "град София, Захарна фабрика",
            "град София, Зона Б-18",
            "град София, Зона Б-19",
            "град София, Зона Б-5",
            "град София, Зона Б-5-3",
            "град София, Иван Вазов",
            "град София, Изгрев",
            "град София, Изток",
            "град София, Илинден",
            "град София, Карпузица",
            "град София, Княжево",
            "град София, Красна поляна 1",
            "град София, Красна поляна 2",
            "град София, Красна поляна 3",
            "град София, Красно село",
            "град София, Кремиковци",
            "град София, Кръстова вада",
            "град София, Лагера",
            "град София, Левски",
            "град София, Левски В",
            "град София, Левски Г",
            "град София, Лозенец",
            "град София, Люлин - център",
            "град София, Люлин 1",
            "град София, Люлин 2",
            "град София, Люлин 3",
            "град София, Люлин 4",
            "град София, Люлин 5",
            "град София, Люлин 6",
            "град София, Люлин 7",
            "град София, Люлин 8",
            "град София, Люлин 9",
            "град София, Люлин 10",
            "град София, Манастирски ливади",
            "град София, Медицинска академия",
            "град София, Младост 1",
            "град София, Младост 1А",
            "град София, Младост 2",
            "град София, Младост 3",
            "град София, Младост 4",
            "град София, Модерно предградие",
            "град София, Мусагеница",
            "град София, Надежда 1",
            "град София, Надежда 2",
            "град София, Надежда 3",
            "град София, Надежда 4",
            "град София, Обеля",
            "град София, Обеля 1",
            "град София, Обеля 2",
            "град София, Оборище",
            "град София, Овча купел",
            "град София, Овча купел 1",
            "град София, Овча купел 2",
            "град София, Орландовци",
            "град София, Павлово",
            "град София, Подуяне",
            "град София, Полигона",
            "град София, Разсадника",
            "град София, Редута",
            "град София, Света Троица",
            "град София, Световрачене",
            "град София, Свобода",
            "град София, Сердика",
            "град София, Симеоново",
            "град София, Славия",
            "град София, Слатина",
            "град София, Стрелбище",
            "град София, Студентски град",
            "град София, Сухата река",
            "град София, Суходол",
            "град София, Толстой",
            "град София, Триъгълника",
            "град София, Факултета",
            "град София, Фондови жилища",
            "град София, Хаджи Димитър",
            "град София, Хиподрума",
            "град София, Хладилника",
            "град София, Център",
            "град София, Яворов",
            "град София, м-т Гърдова глава",
            "град София, м-т Детски град",
            "град София, м-т Киноцентъра",
            "град София, Малинова долина",
            "град София, в.з.Американски колеж",
            "град София, в.з.Бункера",
            "град София, в.з.Врана - Лозен",
            "град София, в.з.Киноцентъра",
            "град София, в.з.Киноцентъра 3 част",
            "град София, в.з.Малинова долина",
            "град София, в.з.Симеоново - Драгалевци",
        };



    }
}
