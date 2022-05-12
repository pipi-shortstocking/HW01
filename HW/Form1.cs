using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;

namespace HW
{
    public partial class Form1 : Form
    {
        //그림 불러오기
        Bitmap img1 = new Bitmap("img/1.png");
        Bitmap img2 = new Bitmap("img/2.png");
        Bitmap img3 = new Bitmap("img/3.png");
        Bitmap img4 = new Bitmap("img/4.png");
        Bitmap img5 = new Bitmap("img/5.png");
        Bitmap img6 = new Bitmap("img/6.png");
        Bitmap img7 = new Bitmap("img/7.png");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string query = "http://www.kma.go.kr/wid/queryDFSRSS.jsp?zone=" + textBox1.Text;

            //먼저 클라이언트에서 request 한다!
            WebRequest wr = WebRequest.Create(query);
            wr.Method = "GET";

            //Response를 받는다!
            WebResponse wrs = wr.GetResponse();
            Stream s = wrs.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            string response = sr.ReadToEnd();

            //richTextBox1.Text = response;

            //response 받은 것을 xml 파싱한다!

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(response);

            //제목, 중요한 정보 처리
            XmlNode channel = xd["rss"]["channel"];
            label65.Text = channel["title"].InnerText;
            label66.Text = channel["pubDate"].InnerText;

            //데이터 처리
            XmlNode xn = xd["rss"]["channel"]["item"]["description"]["body"];

            /*
             *  시간 hour
             *  기온 temp
             *  날씨 wfKor
             *  강수확률 pop
             *  풍속 ws
             *  풍향 wdKor
             *  습도 reh
             */

            Label[] hour = { label2, label9, label16, label23, label30, label37, label44, label51, label58 };
            Label[] temp = { label3, label10, label17, label24, label31, label38, label45, label52, label59 };
            Label[] wfKor = { label4, label11, label18, label25, label32, label39, label46, label53, label60 };
            Label[] pop = { label5, label12, label19, label26, label33, label40, label47, label54, label61 };
            Label[] ws = { label6, label13, label20, label27, label34, label41, label48, label55, label62 };
            Label[] wdKor = { label7, label14, label21, label28, label35, label42, label49, label56, label63 };
            Label[] reh = { label8, label15, label22, label29, label36, label43, label50, label57, label64 };

            PictureBox[] pb = { 
                pictureBox1, 
                pictureBox2, 
                pictureBox3, 
                pictureBox4,
                pictureBox5,
                pictureBox6,
                pictureBox7,
                pictureBox8,
                pictureBox9
            };

            //원래 차트에 그려져 있던 내용 초기화
            chart1.Series[0].Points.Clear();

            //차트 디자인 설정
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 8;

            CustomLabel[] cl = new CustomLabel[9];

            for (int i = 0; i < 9; i++)
            {
                //richTextBox1.Text += xn.ChildNodes[i]["hour"].InnerText + "\n";
                hour[i].Text = "시간:" + xn.ChildNodes[i]["hour"].InnerText;
                pop[i].Text = "강수확률:" + xn.ChildNodes[i]["pop"].InnerText + "%";
                ws[i].Text = "풍속:" + xn.ChildNodes[i]["ws"].InnerText + "m/s";
                wdKor[i].Text = "풍향:" + xn.ChildNodes[i]["wdKor"].InnerText;
                reh[i].Text = "습도" + xn.ChildNodes[i]["reh"].InnerText + "%";

                temp[i].Text = "기온:" + xn.ChildNodes[i]["temp"].InnerText + "'C";

                double graph_temp = double.Parse(xn.ChildNodes[i]["temp"].InnerText);
                //기온으로 그래프 그리기
                chart1.Series[0].Points.AddXY(i, graph_temp);
                cl[i] = new CustomLabel();
                cl[i].Text = xn.ChildNodes[i]["hour"].InnerText;
                cl[i].FromPosition = i - 1;
                cl[i].ToPosition = i + 1;
                chart1.ChartAreas[0].AxisX.CustomLabels.Add(cl[i]);

                string wf = xn.ChildNodes[i]["wfKor"].InnerText;
                wfKor[i].Text = "날씨:" +wf;

                if(wf == "맑음")
                {
                    pb[i].Image = img1;
                }
                else if(wf == "구름 많음")
                {
                    pb[i].Image = img2;
                }
                else if (wf == "흐림")
                {
                    pb[i].Image = img3;
                }
                else if (wf == "비")
                {
                    pb[i].Image = img4;
                }
                else if (wf == "비/눈")
                {
                    pb[i].Image = img5;
                }
                else if (wf == "눈")
                {
                    pb[i].Image = img6;
                }
                else if (wf == "소나기")
                {
                    pb[i].Image = img7;
                }
                //날씨에 따라서 
            }
            
        }

    }
}
