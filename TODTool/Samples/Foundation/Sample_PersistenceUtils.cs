using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPersistence;

namespace TODTool
{
    public class PersistenceUtils
    {
        private static EntityManager em;
        private static EntityManagerFactory emf;
        public static EntityManager getEm()
        {
            if (em == null)
            {
                emf = Persistence.CreateEntityManagerFactory("webAppPersistenceUnit");
                em = emf.CreateEntityManager();
            }
            if (!em.IsOpen())
            {
                em = emf.CreateEntityManager();
            }
            return em;
        }
        internal static EntityManagerFactory getEmf()
        {
            return emf;
        }
    }
}