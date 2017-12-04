using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.Windows.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.IO;

namespace VG_List
{
    public enum VGStatus
    {
        Пройдена,
        Играю,
        Отложена,
        Заброшена,
        Поиграть
    }    

    public class VGameList
    {
        public List<VGame> VGames { get; set; }

        public List<string> CatDictionary;

        private string path = "VG-list.xml";
        private string pathHtml = "web\\VG-list-web.html";

        public VGameList()
        {
            VGames = new List<VGame>();
            CatDictionary = new List<string>();
            
        }

        private void FillCatDictionary()
        {
            for (int i = 0; i < VGames.Count; i++)
            {
                if (!(CatDictionary.Contains(VGames[i].Category)) && (VGames[i].Category != null))
                    CatDictionary.Add(VGames[i].Category);
            }
        }

        public void DeserializeVGList()
        {
            try
            {
                XmlSerializer srz = new XmlSerializer(typeof(List<VGame>));
                XmlReader rdr = XmlReader.Create(path);
                VGames = srz.Deserialize(rdr) as List<VGame>;
                rdr.Close();
                FillCatDictionary();
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                VGames = new List<VGame>();
            }
        }

        public void SerializeVGList()
        {
            try
            {
                XmlSerializer srz = new XmlSerializer(typeof(List<VGame>));
                XmlWriterSettings wrtset = new XmlWriterSettings();
                wrtset.Indent = true;
                XmlWriter wrt = XmlWriter.Create(path, wrtset);
                srz.Serialize(wrt, VGames);
                wrt.Close();

                CreateVgHtmlTable();
            }
            catch (Exception exc)
            {
                if (VGames == null)
                    VGames = new List<VGame>();
            }
        }

        public void CreateVgHtmlTable()
        {
            var table = new HtmlTable();
            //var mailMessage = new StringBuilder();
            string html = "<html><head><title>VG list</title><script type=\"text/javascript\" src=\"jquery-3.2.1.min.js\"></script><script type=\"text/javascript\" src=\"sorttable.js\"></script></head><body>\n";
            HtmlTableRow row;

            foreach (VGame vg in VGames)
            {
                row = new HtmlTableRow();
                row.Cells.Add(new HtmlTableCell { InnerText = vg.Title });
                //row.Cells.Add(new HtmlTableCell { InnerText = vg.AltTitle });
                row.Cells.Add(new HtmlTableCell { InnerText = vg.Platform });
                row.Cells.Add(new HtmlTableCell { InnerText = vg.Status.ToString() });
                row.Cells.Add(new HtmlTableCell { InnerText = vg.ScoreStr });
                row.Cells.Add(new HtmlTableCell { InnerText = vg.DateFinishedStr });
                table.Rows.Add(row);
            }

            using (var sw = new StringWriter())
            {
                table.RenderControl(new HtmlTextWriter(sw));
                html += sw.ToString().Insert(7, "<tr><th>Title</th><th>Platform</th><th>Status</th><th>Status</th><th>Date of finishing</th></tr>").Insert(6, " class=\"sortable\" border=\"2\"");

            }
            html += "</body></html>";
            System.IO.File.WriteAllText(pathHtml, html);
        }
    }
}
