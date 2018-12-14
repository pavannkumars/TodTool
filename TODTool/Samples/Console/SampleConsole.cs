using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPersistence;

namespace TODTool
{
    public class SampleConsole
    {
        private static void createAndSaveEntity()
        {
            EntityManager em = PersistenceUtils.getEm();
            Employee jon = new Employee();
            jon.FirstName = "jon";
            jon.LastName = "do";
            
            em.GetTransaction().Begin();
            em.Persist(jon);
            em.GetTransaction().Commit();

            TypedQuery<Employee> query = em.CreateQuery<Employee>("from Employee as emp where emp.FirstName = ?", typeof(Employee));
            query.SetParameter(0, jon.FirstName);
            Employee emp = query.GetSingleResult();

            Console.WriteLine("Hello" + emp.FirstName);
        }
    }
}