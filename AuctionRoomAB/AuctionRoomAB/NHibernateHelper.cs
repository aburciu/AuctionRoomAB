using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Configuration;

namespace AuctionRoomAB
{
    public class NHibernateHelper
    {
        private static ISessionFactory SessionFactory;
        public static string ConnectionString { get; private set; }

        public static void Initialize()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            CreateSessionFactory();
        }

        public static ISessionFactory CurrentSessionFactory { get { return SessionFactory; } }

        private static void CreateSessionFactory()
        {
            if (SessionFactory != null) return;

            try
            {
                SessionFactory = Fluently.Configure()
                    .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, TimeSpan.FromMinutes(5).TotalSeconds.ToString()))
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString))//.ShowSql)
                    .Mappings(m =>
                    {
                        m.HbmMappings.AddFromAssembly(Assembly.GetExecutingAssembly());
                        m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly());
                    })
                    .BuildSessionFactory();
            }
            catch (FluentConfigurationException configEx)
            {
                throw configEx;
            }
        }
    }
}
