using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VeterinaryClinic.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    ServicesId = table.Column<List<int>>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    LongDescription = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    ReceptionTimeMinutes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    TypeOfAnimal = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    TypeOfService = table.Column<string>(type: "text", nullable: true),
                    DateTimeStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateTimeEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    StatusCode = table.Column<int>(type: "integer", nullable: false),
                    MessageForUser = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "FullName", "ServicesId" },
                values: new object[,]
                {
                    { 1, "Толасова Виктория Викторовна", new List<int> { 1, 2, 3, 4, 5, 6 } },
                    { 2, "Лаптев Валерий Викторович", new List<int> { 1, 2, 3, 4, 5, 6 } },
                    { 3, "Морозов Александр Васильевич", new List<int> { 1, 2, 3, 4, 5, 6 } },
                    { 4, "Мамлеева Аделя Рифкатовна", new List<int> { 1, 2, 3, 4, 5, 6 } },
                    { 5, "Куркурин Николай Дмитриевич", new List<int> { 1, 2, 3, 4, 5, 6 } },
                    { 6, "Лайко Наталья Владимировна", new List<int> { 1, 2, 3, 4, 5, 6 } }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "PetOwner" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "LongDescription", "Name", "Price", "ReceptionTimeMinutes", "ShortDescription" },
                values: new object[,]
                {
                    { 1, "Введение вакцины вызывает выработку организмом животного антител против вирусных болезней. Есть несколько основных опасных инфекционных болезней: у собак — это чума плотоядных, гепатит, парвовироз, лептоспироз, бешенство; у кошек — вирусный ринотрахеит, калицивироз, панлейкопения кошек и бешенство.Наиболее подвержены риску заражения животные, гуляющие на улице, хотя домашние животные, которые не имеют контакта с внешней средой, также могут подхватить любую инфекцию.Владелец, придя домой с улицы, на своей обуви и одежде может принести любые бактерии, вирусы или яйца гельминтов.", "Вакцинация", 500, 20, "Прививки для кошек и собак с выдачей ветеринарного паспорта." },
                    { 2, "Кастрация и стерилизация — это профилактика болезней матки, яичников, молочных желез у сук и кошек. У кобелей — профилактика болезней по мужской линии, в частности, простатита, аденомы простаты. По медицинским показаниям кастрация проводится достаточно часто: при гнойных воспалениях, опухолях женских органов, кист яичников. Зачастую это единственный способ лечения сахарного диабета, гормональных нозологий, а также действенное средство для избавления кобелей и котов от некоторых дерматологических заболеваний, патологий семенников, других органов размножения. Например, при хирургическом лечении мочекаменной болезни, промежностной грыжи у кобелей мелких пород кастрация также обязательна, — рассказывает доктор ветслужбы Захаров и Фарафонтова Алексей Кулишов. — После операции, поверьте, животные, в отличие от человека, не испытывают чувства неполноценности, угрызений совести по поводу отсутствия либидо. Кастрация и стерилизация продлевают жизнь и улучшают ее качество!", "Стерилизация", 1000, 60, "Кастрация котов и собак, операции по стерилизации кошек и собак" },
                    { 3, "Высочайшая квалификация врачей-хирургов позволяет проводить сложные хирургические манипуляции, например: остеосинтез и создание искусственных связок, абдоминальная хирургия, пластическая хирургия и многое другое. Владельцу животного необходимо осознавать,что общая анестезия(наркоз) и оперативное вмешательство связано с определенной степенью риска для животного,что может вызвать послеоперационные осложнения или неблагоприятный исход операции. Поэтому перед операцией животное должно быть обследовано, для того, чтобы удостовериться, что наркоз не принесёт животному вреда. Обследование животного перед операцией, которая проводится под общим наркозом, должно включать следующее: ЭКГ, УЗИ, Общий клинический анализ крови, биохимия крови.", "Хирургия", 10000, 120, "Хирургическое лечение опухолей, травм, врожденных нарушений." },
                    { 4, "Уважаемые пациенты клиники! Вы можете круглосуточно получить бесплатную консультацию ветеринара по телефону. Вы можете задать вопрос ветеринарам клиники, которые постоянно находятся по указанному номеру и готовы помочь советом, проанализировать признаки заболевания, оказать посильную ветеринарную помощь по телефону.Убедительно просим Вас учесть тот факт, что невозможно вылечить или поставить точный диагноз заочно, не осмотрев животное, не проведя необходимых исследований.Поэтому ветпомощь по телефону может быть оказана врачом лишь в ограниченном объеме.Консультация ветеринара по телефону охватывает вопросы здоровья, содержания, кормления животных, аналогов препаратов, схемы вакцинации, необходимости стерилизации, проведения хирургического лечения и т.д. Консультация ветеринара по телефону – это такой вид консультации, который является первоочередным в ситуациях, когда животное нуждается в срочной помощи. Получив советы ветеринара, Вы будете четко знать, как действовать дальше.", "Консультации", 100, 30, "Советы по лечению и уходу за домашними животными. Назначение лекарств." },
                    { 5, "Иногда лечение животных лучше осуществлять на дому. Перевозка – причины сильного стресса, что плохо сказывается на его общем состоянии. Своевременное реагирование медперсонала на экстренные вызовы помогает сэкономить драгоценное время. От этого часто зависит жизнь питомца. Если иммунитет любимца снижен, при контакте с другими животными в ветклинике существует вероятность подхватить новое заболевание. Выезд опытного ветеринарного врача на дом исключает риск повторного заражения. Привычная домашняя обстановка обеспечивает высокий уровень комфорта, зверек ведет себя спокойно. Ветеринары беспрепятственно произведут осмотр и установят максимально точный диагноз. В зависимости от вида болезни и питомца, на дом выезжают ветеринары различной направленности: герпетолог, ратолог, орнитолог, хирург, офтальмолог, онколог и другие ветдоктора. Выезд узкого специалиста на указанный адрес возможен круглосуточно.", "Лечение на дому", 5000, 120, "Выезд ветеринара к вам домой.Осмотр и проведение операции на дому.Послеоперационное наблюдение." },
                    { 6, "Важность косметических процедур у животных многие недооценивают. Но, как говорится: «незнание закона не освобождает от отвественности». Невыполнение некоторых простых манипуляций может вести к достаточно серьёзным последствиям. Наши услуги: чистка ушей, подрезание когтей, чистка ПАЖ и многое другое", "Косметические операции", 5000, 120, "Стрижка собак и кошек. Удаление когтей. Купирование ушей и хвостов" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "RoleId" },
                values: new object[] { 1, "gorstsergei@mail.ru", "12345", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
