using MyKnitting.Services;
using MyKnitting.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Data.Sqlite;
using System.IO;
using MyKnitting.Models;

namespace MyKnitting {
    public partial class App : Application {

        public App() {
            InitializeComponent();

            DependencyService.Register<ProjectDataStore>();
            DependencyService.Register<NeedleDataStore>();
            DependencyService.Register<YarnDataStore>();
            DependencyService.Register<NFPDataStore>();
            DependencyService.Register<YFPDataStore>();

            
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(basePath, "database.db");


            if (!File.Exists(databasePath)) {

                new SQLite.SQLiteConnection(databasePath);

                var db = new SqliteConnection("Data source=" + databasePath);
                db.Open();
                var command = db.CreateCommand();
                command.CommandText =
                    @"create table needles
                      (
                       id     integer not null
                        constraint " + "\"needles_pk\"" + @"
                        primary key autoincrement,
                        type   text,
                        size   integer,
                        length integer,
                        owned  text
                    );

                    create unique index "+ "\"needles_id_uindex\"" + @"
                        on needles(id); ";

                command.ExecuteNonQuery();

                command = db.CreateCommand();
                command.CommandText =
                    @"create table projects 
                    (
                        id      integer not null
                                constraint " + "\"projects _pk\"" + @"
                                primary key autoincrement,
                        name    text,
                        photo   text,
                        pattern text,
                        done    text
                    );

                    create unique index " + "\"projects _id_uindex\"" +
                        "on projects (id); ";

                command.ExecuteNonQuery();
                
                command = db.CreateCommand();
                command.CommandText =
                    @"create table yarns
                    (
                        id        integer not null
                                  constraint yarns_pk
                                  primary key autoincrement,
                        name    text,
                        color text,
                        colorcode text,
                        size integer,
                        material  text,
                        amount integer,
                        owned     text
                    );

                    create unique index yarns_id_uindex
                    on yarns(id); ";
                command.ExecuteNonQuery();

                command = db.CreateCommand();
                command.CommandText =
                    @"create table yfp
                    (
                        id        integer not null
                                  constraint yfp_pk
                                  primary key autoincrement,
                        projectid integer
                                references projects,
                        yarnid integer
                                references yarns
                    );

                    create unique index yfp_id_uindex
                    on yfp(id); ";
                command.ExecuteNonQuery();

                command = db.CreateCommand();
                command.CommandText = @"
                    create table nfp
                    (
                        id        integer not null
                                  constraint nfp_pk
                                  primary key autoincrement,
                        projectid integer
                                references projects,
                        needleid  integer
                                references needles
                    );

                    create unique index nfp_id_uindex
                    on nfp(id);";
                command.ExecuteNonQuery();
                db.Close();
            }
               

            MainPage = new AppShell();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
