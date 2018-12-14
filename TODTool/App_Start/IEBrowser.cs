

using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Collections.Generic;
using TODTool.TODSchedulers;
using System.Linq;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using TODTool.Helpers;
using System.Data.Entity;

namespace TODTool.App_Start
{
    public class IEBrowser : System.Windows.Forms.ApplicationContext
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AutoResetEvent resultEvent;

        public string userName, password;
        public string htmlResult;
        public string htmlCookieTable;
        public string htmlInputTable;
        public string htmlScriptTable;

        //Boolean ceUrlNavigated = false;

        public int loginCount;
        public int navigationCounter;
        ScriptCallback scriptCallback;

        public WebBrowser ieBrowser;
        public Form form;
        public Thread thrd;

        static string ceUrl = "https://dellreach07.sdlproducts.com/LiveContent/web/xforms.xql?&action=view&format=xml&parent=ROOT&fid=xform.comment&c=t&start=0&length=10000";
        static string signinUrl = "https://dellcms.sdlproducts.com/ISHSTS/account/signin";
        static string singintokenUrl = "https://dellcms.sdlproducts.com/ISHSTS";

        public string HtmlResult
        {
            get { return htmlResult; }
        }

        public int NavigationCounter
        {
            get { return navigationCounter; }
        }

        public string HtmlCookieTable
        {
            get { return "Cookies:" + htmlCookieTable; }
        }

        public string HtmlInputTable
        {
            get { return "Input elements:" + htmlInputTable; }
        }

        public string HtmlScriptTable
        {
            get { return "Script variables:" + htmlScriptTable; }
        }


        /// <summary>
        /// class constructor 
        /// </summary>
        /// <param name="visible">whether or not the form and the WebBrowser control are visiable</param>
        /// <param name="userName">client user name</param>
        /// <param name="password">client password</param>
        /// <param name="resultEvent">functionality to keep the main thread waiting</param>
        public IEBrowser(bool visible, string userName, string password, AutoResetEvent resultEvent)
        {
            this.userName = userName;
            this.password = password;

            this.resultEvent = resultEvent;
            htmlResult = null;


            thrd = new Thread(new ThreadStart(
                delegate {
                    try
                    {
                        Init(visible);
                        System.Windows.Forms.Application.Run(this);
                    }
                    catch (Exception e)
                    {
                        log.Info("IE Browser Initialization failed with " + e.StackTrace);
                    }
                }));
            // set thread to STA state before starting
            thrd.SetApartmentState(ApartmentState.STA);
            thrd.Start();

        }

        // initialize the WebBrowser and the form
        private void Init(bool visible)
        {
            try
            {
                scriptCallback = new ScriptCallback(this);

                // create a WebBrowser control
                ieBrowser = new WebBrowser();

                // set the location of script callback functions
                //ieBrowser.ObjectForScripting = scriptCallback;
                // set WebBrowser event handls
                ieBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(IEBrowser_DocumentCompleted);
                ieBrowser.Navigating += new WebBrowserNavigatingEventHandler(IEBrowser_Navigating);
                //ieBrowser.Navigated += new WebBrowserNavigatedEventHandler(IEBrowser_Navigated);
                if (visible)
                {
                    form = new System.Windows.Forms.Form();
                    ieBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
                    form.Controls.Add(ieBrowser);
                    form.Visible = true;
                }

                loginCount = 0;
                // initialise the navigation counter
                navigationCounter = 0;
                /*ieBrowser.Navigate("http://login.live.com");*/
                ieBrowser.Navigate(signinUrl);
            }
            catch (Exception e)
            {
                log.Info("Init of Browser Failed. Error is : " + e.StackTrace);
            }


        }

        private void IEBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string navigatedUrl = e.Url.ToString();
            if (!navigatedUrl.Contains(signinUrl) && navigatedUrl.Equals(ceUrl))
            {
                //ceUrlNavigated = true;
            }
        }

        public void DisposeBrowser(bool disposing)
        {
            try
            {
                Dispose(disposing);
            }
            catch (Exception e)
            {
                log.Info("Exception Occured.");
            }

        }

        // dipose the WebBrowser control and the form and its controls
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (thrd != null)
                {
                    //log.Info("Comment Extraction Execution completed at : " + DateTime.Now + ". Scheduled next @ " + (DateTime.Now).AddDays(7));
                    try
                    {
                        thrd.Abort();
                    }
                    catch (ThreadAbortException e)
                    {
                        //ignore as this is raised by aborting the thread
                    }
                    if (form != null) form.Dispose();
                    ieBrowser.Dispose();
                    thrd = null;
                    //System.Runtime.InteropServices.Marshal.Release(ieBrowser.Handle);

                    //base.Dispose(disposing);*/
                    return;
                }

                /*System.Runtime.InteropServices.Marshal.Release(ieBrowser.Handle);
                ieBrowser.Dispose();
                if (form != null) form.Dispose();
                base.Dispose(disposing);*/
            }
            catch (Exception ee)
            {
                log.Info("CE Extract Browser Dispose Exception Occured with  " + ee.StackTrace);
            }

        }



        // Navigating event handle
        void IEBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // navigation count increases by one
            navigationCounter++;
            // write url into the form's caption
            if (form != null) form.Text = e.Url.ToString();
        }



        // DocumentCompleted event handle
        void IEBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument doc = null;
            string docUrl = ieBrowser.Url.ToString();
            bool isErrorSet = false;
            try
            {
                /*if (doc.Title.Equals("Welcome to Windows Live") && loginCount++ < 3)
                   {
                       // set email address and password, and try to login three times
                       try { doc.GetElementById("i0116").SetAttribute("value", userName); } catch {
                           ieBrowser.Navigate("http://login.live.com/#");
                           return;
                       }
                       doc.GetElementById("i0118").SetAttribute("value", password);
                       doc.GetElementById("idSIButton9").InvokeMember("click");
                   }
                   else
                   {
                       // request jscript to call c# callback function with a parameter of navigation counter
                       doc.InvokeScript("setTimeout", new object[] { string.Format("window.external.getHtmlResult({0})", navigationCounter), 10 });
                   }*/
                if (docUrl.StartsWith(signinUrl))
                {
                    doc = ((WebBrowser)sender).Document;

                    doc.GetElementById("UserName").SetAttribute("value", userName);
                    doc.GetElementById("Password").SetAttribute("value", password);
                    doc.GetElementById("loginButton").InvokeMember("click");


                }
            }
            catch (Exception re)
            {
                log.Info("Sign-In to Reach Server failed. Error is : " + re.StackTrace);
                isErrorSet = true;
            }


            if (!isErrorSet)
            {
                try
                {
                    if (docUrl.Contains(singintokenUrl))
                    {
                        ieBrowser.Navigate(ceUrl);
                        isErrorSet = false;
                    }
                }
                catch (Exception ne)
                {
                    log.Info("Reach Server Navigation failed. Error is : " + ne.StackTrace);
                    isErrorSet = true;
                }



                if (!isErrorSet)
                {
                    // request jscript to call c# callback function with a parameter of navigation counter
                    //doc.InvokeScript("setTimeout", new object[] { string.Format("window.external.getHtmlResult({0})", navigationCounter), 10 });*/
                    if (docUrl.Equals(ceUrl))
                    {
                        string tempDir = "C:/IDTools/temp";
                        DirectoryInfo di;
                        string tempFileName = null;
                        String tempFile = "/tempReachFormat.xml";
                        XmlDocument xmlDoc = null;
                        XmlNodeList xnList = null;
                        try
                        {

                            if (!Directory.Exists(tempDir))
                            {
                                di = Directory.CreateDirectory(tempDir);
                                tempFileName = di.FullName + tempFile;
                            }
                            else
                            {
                                tempFileName = tempDir + tempFile;
                            }
                            StreamWriter writer = File.CreateText(tempFileName);
                            writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");

                            //.Replace("- <", "<").Replace("&","&amp;"));
                            StringBuilder strb = new StringBuilder(ieBrowser.Document.Body.InnerText.ToString());
                            strb = strb.Replace("&", "&amp;");
                            strb = strb.Replace("- <", "<").Replace("&", "&amp;");

                            strb = strb.Replace("&", "&amp;");
                            strb = strb.Replace("<", "&lt;");
                            strb = strb.Replace(">", "&gt;");
                            strb = strb.Replace("&lt;lcform", "<lcform");
                            strb = strb.Replace("&lt;/lcform&gt;", "</lcform>");
                            strb = strb.Replace("&lt;name", "<name");
                            strb = strb.Replace("&lt;/name&gt;", "</name>");
                            strb = strb.Replace("&lt;email", "<email");
                            strb = strb.Replace("&lt;/email&gt;", "</email>");
                            strb = strb.Replace("&lt;summary", "<summary");
                            strb = strb.Replace("&lt;/summary&gt;", "</summary>");
                            strb = strb.Replace("&lt;description", "<description");
                            strb = strb.Replace("&lt;/description&gt;", "</description>");
                            strb = strb.Replace("&lt;reftext", "<reftext");
                            strb = strb.Replace("&lt;/reftext&gt;", "</reftext>");
                            strb = strb.Replace("&lt;/xform&gt;", "</xform>");
                            strb = strb.Replace("&lt;/instances&gt;", "</instances>");
                            strb = strb.Replace("&lt;instances", "<instances");
                            strb = strb.Replace("&lt;xform", "<xform");
                            strb = strb.Replace("\" /&gt;", "\" />");
                            strb = strb.Replace("\"&gt;", "\">");
                            strb = strb.Replace("topic_title=\"\"?\"and \"? &lt;subcommand&gt;\"\"", "topic_title=\"  &lt;subcommand&gt;\"");
                            strb = strb.Replace("topic_title=\"Befehlszeilenhandbuch \"CLI Reference\"\"", "topic_title=\"  Befehlszeilenhandbuch CLI Reference\"");


                            writer.Write(strb.ToString());


                            xmlDoc = new XmlDocument();
                            /**string m = webBrowser1.Document.Body.InnerText.ToString().Replace("- <", "<");
                            m = m.Replace("&", "&amp;");
                            m = m.Replace("<","&lt;");
                            m = m.Replace(">", "&gt;");
                            m = m.Replace("&lt;lcform", "<lcform");
                            m = m.Replace("&lt;/lcform&gt;", "</lcform>");
                            m = m.Replace("&lt;name", "<name");
                            m = m.Replace("&lt;/name&gt;", "</name>");
                            m = m.Replace("&lt;email", "<email");
                            m = m.Replace("&lt;/email&gt;", "</email>");
                            m = m.Replace("&lt;summary", "<summary");
                            m = m.Replace("&lt;/summary&gt;", "</summary>");
                            m = m.Replace("&lt;description", "<description");
                            m = m.Replace("&lt;/description&gt;", "</description>");
                            m = m.Replace("&lt;reftext", "<reftext");
                            m = m.Replace("&lt;/reftext&gt;", "</reftext>");
                            m = m.Replace("&lt;/xform&gt;", "</xform>");
                            m = m.Replace("&lt;/instances&gt;", "</instances>");
                            m = m.Replace("&lt;instances", "<instances");
                            m = m.Replace("&lt;xform", "<xform");
                            m = m.Replace("\" /&gt;", "\" />");
                            m = m.Replace("\"&gt;","\">");
                            m = m.Replace("topic_title=\"\"?\"and \"? &lt;subcommand&gt;\"\"", "topic_title=\"?and ? &lt;subcommand&gt;\"");
                            
                            /* temp to date the strings
                            DateTime tempdelete = Convert.ToDateTime("02/01/2016");
                            string s2 = tempdelete.ToString("yyyy-MM-dd");
                            DateTime dtnew = Convert.ToDateTime(s2); */
                            //MessageBox.Show(dtnew.AddDays(-7).ToString("MM-dd-yyyy"));
                            // System.Console.Write(m);**/
                            //   doc.Save(writer);
                            writer.Close();
                            xmlDoc.Load(tempFileName);
                            xnList = xmlDoc.SelectNodes("//lcform");
                            //File.Delete("LCR_Comments_" + String.Format("{0:yyyy_MM_dd}", DateTime.Today) + ".xlsx");
                            //FileInfo newFile = new FileInfo(@"D:\temp\LCR_Comments_" + String.Format("{0:yyyy_MM_dd}", DateTime.Today) + ".xlsx");
                            //FileInfo newFile = new FileInfo(@"LCR_Comments_" + String.Format("{0:yyyy_MM_dd}", DateTime.Today) + ".xlsx");
                            //ieBrowser.Dispose();
                            isErrorSet = false;
                            log.Info("Comment Extraction completed Successfully from Reach Server.");
                        }
                        catch (Exception ce)
                        {
                            log.Info("Comment Extraction failed. Error is : " + ce.StackTrace);
                            isErrorSet = true;
                        }

                        string schlogMessage = "";
                        //Save the Comments Extracted into the database
                        if (!isErrorSet)
                        {
                            try
                            {
                                log.Info("Comment Extracted Data saving to Database initiated.");
                                DateTime currDate = TodDateUtils.GetCurrentTimeInIST();
                                string fiscalweek = TodDateUtils.GetWeekOfYearFor(TodDateUtils.GetCurrentTimeInIST());
                                DateTime ceStartDate = currDate.AddDays(-7).Date;
                                DateTime ceEndDate = currDate.AddDays(-1).Date;
                                FW_CALENDAR fwcal = null;


                                using (var ctx = new TODEntities())
                                {
                                    fwcal = new FW_CALENDAR();
                                    fwcal.ID = 1;
                                    fwcal.Fiscal_Week = "FW" + fiscalweek;

                                    ctx.Entry<FW_CALENDAR>(fwcal).State = fwcal.Fiscal_Week == null ? EntityState.Added : EntityState.Unchanged;

                                    ctx.SaveChanges();

                                    var ceDataList = new List<CE_DATA>();
                                    foreach (XmlNode xn in xnList)
                                    {
                                        //prepare the cedata set based on the condition 
                                        string tempDate = xn.Attributes["created"].Value;
                                        DateTime dt = Convert.ToDateTime(tempDate);
                                        if (dt.Date >= ceStartDate && dt.Date != DateTime.Now.Date)
                                        {
                                            CE_DATA cedataset = new CE_DATA();

                                            cedataset.FW_CALENDAR = fwcal;
                                            cedataset.FW_ID = fwcal.Fiscal_Week;

                                            cedataset.REP_GEN_DATE = currDate;

                                            cedataset.CREATED_DATE = dt;
                                            cedataset.CREATED_TIME = dt;
                                            cedataset.FW_START_DATE = ceStartDate;
                                            cedataset.FW_END_DATE = ceEndDate;

                                            cedataset = processNodeAttributes(cedataset, xn);

                                            string tempText = xn.InnerText;
                                            string[] words = tempText.Split('|');
                                            int interCoun = 7;
                                            for (int i = 0; i < words.Length; i++)
                                            {
                                                if (i != 0 && i % 2 == 0)
                                                {
                                                    if (interCoun < 10)
                                                    {
                                                        //worksheet.Cells[columnCount, interCoun].Style.Numberformat.Format = "###0";
                                                        int temp = int.Parse(words[i]);

                                                        //worksheet.Cells[columnCount, interCoun].Value = temp;
                                                        switch (interCoun)
                                                        {
                                                            case 7:
                                                                //accurate rating
                                                                cedataset.ACCURATE_RATING = temp;
                                                                break;
                                                            case 8:
                                                                //useful rating
                                                                cedataset.USEFUL_RATING = temp;
                                                                break;
                                                            case 9:
                                                                //easy to understand rating
                                                                cedataset.EASY_TO_UNDERSTAND_RATING = temp;
                                                                break;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        switch (interCoun)
                                                        {
                                                            case 10:
                                                                //Article Resolved Issue
                                                                cedataset.ARTICLE_SOLVED_ISSUE = words[i];
                                                                break;
                                                            case 11://feedback
                                                                cedataset.FEEDBACK = words[i];
                                                                break;
                                                        }

                                                        //worksheet.Cells[columnCount, interCoun].Value = words[i];
                                                    }
                                                    interCoun++;
                                                }
                                            }
                                            cedataset.CE_STATUS = "Success";
                                            ctx.Entry<CE_DATA>(cedataset).State = cedataset.CE_SEQUENCE_ID == 0 ? EntityState.Added : EntityState.Unchanged;
                                            ceDataList.Add(cedataset);

                                        }
                                    }

                                    //attach the disconnected entities
                                    ctx.CE_DATA.AddRange(ceDataList);

                                    //save the cedata set to database
                                    ctx.SaveChanges();

                                    schlogMessage = "Comment Extraction scheduled @ " + currDate + " successfully extracted " + ceDataList.Count() + " records. CE Next Scheduled is " + TodDateUtils.GetFutureTimeInIST(7);
                                    TodScheduler.schedulerJobMessage = schlogMessage;
                                    TodScheduler.schStatus = true;
                                    log.Info(schlogMessage);

                                    //save log info into log table
                                    TOD_TransactionLog todlog = new TOD_TransactionLog();
                                    todlog.logtype = "CEDataLog";
                                    todlog.logdate = DateTime.Now.Date.ToString();
                                    todlog.logMessage = schlogMessage;
                                    todlog.logStatus = true.ToString();
                                    ctx.TOD_TransactionLog.Add(todlog);

                                    ctx.SaveChanges();
                                }

                            }
                            catch (DbEntityValidationException dbev)
                            {
                                schlogMessage = "Comment Extraction scheduled @ " + TodDateUtils.GetCurrentTimeInIST() + " failed. CE Next Scheduled is " + TodDateUtils.GetFutureTimeInIST(7);
                                TodScheduler.schedulerJobMessage = schlogMessage;
                                log.Info(schlogMessage);
                                TodScheduler.schStatus = false;
                                log.Info(dbev.Message);
                                log.Info("Saving CE Data Failed. Exception occured " + dbev.InnerException);
                            }
                            catch (DbUpdateException dbe)
                            {
                                schlogMessage = "Comment Extraction scheduled @ " + TodDateUtils.GetCurrentTimeInIST() + " failed. CE Next Scheduled is " + TodDateUtils.GetFutureTimeInIST(7);
                                TodScheduler.schedulerJobMessage = schlogMessage;
                                log.Info(schlogMessage);

                                TodScheduler.schStatus = false;
                                log.Info(dbe.Message);
                                log.Info("Saving CE Data Failed. Exception occured " + dbe.InnerException);
                            }
                            DisposeBrowser(true);
                        }
                    }
                }

            }
        }

        public FW_CALENDAR CheckIfFWCExists(IList<FW_CALENDAR> cedatalist, String key)
        {
            FW_CALENDAR retCEData = null;
            try
            {
                foreach (var ce in cedatalist)
                {
                    if (ce.Fiscal_Week == key)
                    {
                        //exists
                        retCEData = null;
                    }
                    else
                    {
                        retCEData = ce;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                log.Info("Exception occured whiel checking CEDAta Existatnce.");
            }
            return retCEData;
        }

        public CE_DATA processListAndIFExists(IList<CE_DATA> cedatalist, String key)
        {
            CE_DATA retCEData = null;
            try
            {
                foreach (var ce in cedatalist)
                {
                    if (ce.FW_ID == key)
                    {
                        //exists
                        retCEData = null;
                    }
                    else
                    {
                        retCEData = ce;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                log.Info("Exception occured whiel checking CEDAta Existatnce.");
            }
            return retCEData;
        }

        public CE_DATA processNodeAttributes(CE_DATA cedataset, XmlNode xn)
        {
            cedataset.PUBLICATION = xn.Attributes["pub"].Value;
            cedataset.LANGUAGE = xn.Attributes["lang"].Value;
            cedataset.GUID = xn.Attributes["sdocid"].Value;
            cedataset.Title = xn.Attributes["topic_title"].Value;
            return cedataset;
        }
    }


    /// <summary>
    /// class to hold the functions called by script codes in the WebBrowser control
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ScriptCallback
    {
        IEBrowser owner;
        static string scriptPattern;

        public ScriptCallback(IEBrowser owner)
        {
            this.owner = owner;

            // read JScript.js only once
            if (scriptPattern != null) return;
            StreamReader rd = File.OpenText(System.AppDomain.CurrentDomain.BaseDirectory + "JScript.js");
            scriptPattern = rd.ReadToEnd();
            rd.Close();
        }

        // callback function to get the content of page in the WebBrowser control 
        public void getHtmlResult(int count)
        {
            // unequal means the content is not stable
            if (owner.navigationCounter != count) return;

            // get HTML content
            owner.htmlResult = owner.ieBrowser.DocumentText;

            HtmlDocument doc = owner.ieBrowser.Document;
            if (doc.Cookie != null)
            {
                // get cookies
                owner.htmlCookieTable = "<table border=1 cellspacing=0 cellpadding=2><tr><th>Name</th><th>Value</th><tr>";
                foreach (string cookie in Regex.Split(doc.Cookie, @";\s*"))
                {
                    string[] arr = cookie.Split(new char[] { '=' }, 2);
                    owner.htmlCookieTable += string.Format("<td>{0}</td><td>{1}</td></tr>", arr[0], (arr.Length == 2) ? arr[1] : "&nbsp;");
                }
                owner.htmlCookieTable += "</table><p />";
            }

            HtmlElementCollection inputs = doc.GetElementsByTagName("INPUT");
            if (inputs.Count != 0)
            {
                // get ids, names, values and types of input elements
                owner.htmlInputTable = "<table border=1 cellspacing=0 cellpadding=2><tr><th>Id</th><th>Name</th><th>Value</th><th>Type</th><tr>";
                foreach (HtmlElement input in inputs)
                {
                    owner.htmlInputTable += string.Format("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", input.GetAttribute("Id"), input.GetAttribute("Name"), input.GetAttribute("Value"), input.GetAttribute("Type"));
                }
                owner.htmlInputTable += "</table><p />";
                owner.htmlInputTable = owner.htmlInputTable.Replace("<td></td>", "<td>&nbsp;</td>");
            }

            HtmlElementCollection scripts = doc.GetElementsByTagName("SCRIPT");
            if (scripts.Count != 0)
            {
                string vars = string.Empty;
                foreach (HtmlElement script in scripts)
                {
                    if (script.InnerHtml == null) continue;

                    foreach (string name in getVariableNames(script.InnerHtml).Split(new char[] { ';' }))
                    {
                        if (name.Trim().Length == 0) continue;
                        if (vars.Contains("\"" + name + "\"")) continue;

                        // one row of the script variable table - <tr>getValue([script variable name]</tr> - getValue() is a script function in JScript.js
                        vars += string.Format("+\"<tr>\"+getValue(\"{0}\")+\"</tr>\"", name);
                    }
                }

                // request script to send back the names, values and types of script variables
                doc.InvokeScript("setTimeout", new object[] { scriptPattern.Replace("{0}", vars.Substring(1)), 10 });
            }
            else
            {
                // set resultEvent to let main thread continue
                owner.resultEvent.Set();
            }
        }

        // get script variable names from the InnerHtml of script tag
        string getVariableNames(string InnerHtml)
        {
            // remove fuction definitions
            int n;
            Regex r = new Regex(@"\{|\}");
            while (true)
            {
                Match m1 = Regex.Match(InnerHtml, @"function\s+[^\(]+\(|new\s+function\s*\(|function\s*\(");
                if (!m1.Success) break;

                int nestCount = 0;
                n = m1.Groups[0].Index;
                do
                {
                    Match m2 = r.Match(InnerHtml, n + 1);
                    n = m2.Groups[0].Index;
                    nestCount += (m2.Groups[0].Value[0] == '{') ? 1 : -1;
                } while (nestCount != 0);

                InnerHtml = InnerHtml.Remove(m1.Groups[0].Index, n - m1.Groups[0].Index + 1);
            }

            // remove "if (...)"
            r = new Regex(@"\(|\)");
            while (true)
            {
                Match m1 = Regex.Match(InnerHtml, @"if\s*\(");
                if (!m1.Success) break;

                int nestCount = 0;
                n = m1.Groups[0].Index;
                do
                {
                    Match m2 = r.Match(InnerHtml, n + 1);
                    n = m2.Groups[0].Index;
                    nestCount += (m2.Groups[0].Value[0] == '(') ? 1 : -1;
                } while (nestCount != 0);

                InnerHtml = InnerHtml.Remove(m1.Groups[0].Index, n - m1.Groups[0].Index + 1);
            }

            InnerHtml = Regex.Replace(InnerHtml, @"\W+try\s*\{|\}\s*catch\([^\)]+\)\s*\{|\Welse\W", ";");
            InnerHtml = Regex.Replace(InnerHtml, @"<[^>]+>[^<]*<[^>]+>|<!--[^>]+>", "");
            InnerHtml = Regex.Replace(InnerHtml, @"\+*=[^;\}]+[;\}]|\{|\}", ";");

            string variables = string.Empty;
            //Match m = Regex.Match(InnerHtml, @"(var\s|;+|^|\{+)\s*(\S+?)\s*(=[\s\S]+?[;$]|;|\}|$)");
            r = new Regex(@"(var\s|;+|^)\s*(\S+?)\s*(;+|$)");
            n = 0;
            while (true)
            {
                Match m = r.Match(InnerHtml, n);
                if (!m.Success) break;

                variables += ";" + m.Groups[2].Value;
                n = m.Index + m.Length - 1;
            }

            // remove function calling
            variables = Regex.Replace(variables, @";[^;]+\([^\(;]+", "", RegexOptions.RightToLeft);

            variables = Regex.Replace(variables, @"^\W*;+\s*|\s*;+\W*$", "");
            variables = Regex.Replace(variables, @";{2,}", ";");
            variables = variables.Replace('"', '\'');

            return variables;
        }

        // callback function to set the names, values and types of script variables
        // parameter vars conains the names, values and types of script variables
        public void getScriptResult(string vars)
        {

            // set script table
            owner.htmlScriptTable = "<table border=1 cellspacing=0 cellpadding=2><tr><th>Name</th><th>Value</th><th>Type</th><tr>" + vars + "</table><p />";
            owner.htmlScriptTable = owner.htmlScriptTable.Replace("<td></td>", "<td>&nbsp;</td>");

            // set resultEvent to let main thread continue
            owner.resultEvent.Set();
        }
    }
}