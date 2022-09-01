using AlcoAxe.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Test_OP_Web;
using Test_OP_Web.Data;
using Test_OP_Web.Models;

namespace WebAxe.Data
{
    public class DbInitializer
    {

        static bool Right(string text)
        {
            if (text.Contains("*") || text.Contains("="))
                return true;
            else
                return false;
        }


        private static async Task CreateUserWithoutEmailConfirm(
            UserManager<UserAxe> userManager,
            RoleManager<IdentityRole> roleManager,
            string email,
            string password,
            string role)
        {

            if (await userManager.FindByNameAsync(email) == null)
            {
                UserAxe user = new UserAxe { Email = email, UserName = email };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    await userManager.ConfirmEmailAsync(user, code);

                }
            }

        }




        public static async Task RoleInitialize(UserManager<UserAxe> userManager, RoleManager<IdentityRole> roleManager)
        {



            //Roles
            string adminEmail = "potter27.05@mail.ru";
            string password = "123456Bb!";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }


            await CreateUserWithoutEmailConfirm(userManager, roleManager, adminEmail, password, "admin");

            foreach (var item in Parse().Result)
            {
                await CreateUserWithoutEmailConfirm(userManager, roleManager, item.Email, item.Password, "user");
            }

        }


        public static void Initialize(OptionContext context)
        {


            //if (context.Database.CanConnect())
            //{
            //    Console.WriteLine("Successfully connection to db");

            //}


            //context.Database.EnsureDeleted();



            if (!context.Database.EnsureCreated())
            {
                Console.WriteLine("Db not need created! Allready created");
                return;
            }
            else
            {
                Console.WriteLine("Db successfully created");
            }


            // Создание папки GameQuestions в корне приложения
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(rootPath);


            string safePath;
            string DirectoryName = "GameQuestions";
            if (Directory.Exists(rootPath + DirectoryName))
            {
                safePath = rootPath + DirectoryName;
            }
            else
            {
                Directory.CreateDirectory(rootPath + DirectoryName);
                safePath = rootPath + DirectoryName;
            }


            // Проверка и поиск файлов внутри папки GameQuestions

            string[] allfiles = Directory.GetFiles(safePath, "*.gametype");

            if (allfiles.Length == 0)
            {
                Console.WriteLine("ERROR!\n Missing question files in GameQuestions directory");

            }


            foreach (string fullfilename in allfiles)
            {

                string filename = Path.GetFileNameWithoutExtension(fullfilename);
                Console.WriteLine(filename);

                // Создание GameType

                GameType gameType = new GameType() { NameGameType = filename };

                // Парсинг даных с файла
                if (!File.Exists(fullfilename))
                    return;

                string[] lines = File.ReadAllLines(fullfilename);
                // асинхронное чтение

                for (int i = 0; i < lines.Length; i++)
                {

                    var line = lines[i].Split('*');
                    try
                    {

                        SampleQuestion sampleQuestion = new SampleQuestion()
                        {
                            QuestionString = line[0],
                            GameType = gameType,
                            GameMode = (GameMode)Convert.ToInt32(line[1])
                        };

                        context.Add(sampleQuestion);
                    }
                    catch (Exception)
                    {

                        Console.WriteLine($"error question file.gametype str={line}");
                    }



                }
            }

            context.SaveChanges();
        }

        private static async Task<List<RegUser>> Parse()
        {
            //C:\Users\smallaxe\source\repos\Test_OP_Web\Test_OP_Web\wwwroot\users.json


            string path = "wwwroot\\users.txt";
            List<RegUser> users = new();

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        Console.WriteLine(line);

                        var str = line.Split("\t");

                        users.Add(new RegUser()
                        {
                            UserName = str[0],
                            Password = str[1],
                            SurName = str[2],
                            Name = str[3],
                            Email = str[4].ToLower()
                        }); ;

                    }
                }
            }

            return users;
        }
    }
}

