using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPersistence;

namespace TODTool
{
    public partial class NPA_ExampleForm : System.Web.UI.Page
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Text = "Insert Employee details";
                DisplayAllEmployees();
            }
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            if (txtEmployeeID.Text.Trim().Length > 0)
            {
                Employee newEmployee = new Employee();
                newEmployee.FirstName = txtFirstName.Text;
                newEmployee.ID = txtEmployeeID.Text;
                newEmployee.LastName = txtLastName.Text;


                if (!IsDuplicateOfExisting(newEmployee))
                {
                    EntityManager em = PersistenceUtils.getEm();
                    em.GetTransaction().Begin();
                    try
                    {
                        em.Persist(newEmployee);
                        em.GetTransaction().Commit();
                        Response.Redirect("NPA_ExampleForm.aspx");
                    }
                    catch (Exception e1)
                    {
                        log.Info(e1.Message);
                        lblMessage.Text =
                        "<span style=\"color:red\">Problem during save</span><br />Please try again later.";
                    }
                }
                else
                {
                    lblMessage.Text =
                        "<span style=\"color:red\">The ID you provided is already in use.</span><br />Please change the ID and try again.";
                }
            }
            else
            {
                lblMessage.Text = "<span style=\"color:red\">The ID can't be empty</span>";
            }
        }
        private bool IsDuplicateOfExisting(Employee newEmployee)
        {
            try
            {
                EntityManager em = PersistenceUtils.getEm();
                Employee duplicateEmployee = em.Find<Employee>(typeof(Employee), newEmployee.ID);
                return duplicateEmployee != null;
            }
            catch (Exception e)
            {
                log.Info(e.Message);
                return false;
            }
        }
        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("NPA_ExampleForm.aspx");
        }
        private void DisplayAllEmployees()
        {
            EntityManager em = PersistenceUtils.getEm();
            grdEmployees.DataSource = em.CreateQuery<Employee>("from Employee", typeof(Employee)).GetResultList();
            grdEmployees.DataBind();
        }
    }
}
