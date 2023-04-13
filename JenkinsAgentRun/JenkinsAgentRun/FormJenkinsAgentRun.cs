using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JenkinsAgentRun
{
    public partial class FormJenkinsAgentRun : Form
    {
        public FormJenkinsAgentRun()
        {
            InitializeComponent();
        }

        public delegate void AddConsoleItem(String message);
        public AddConsoleItem myDelegate;
        public Process P;
        private bool help;

        private bool CancelClose = true;

        public void addConsoleItemMethod(String message)
        {
            consoleMessage(message);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            help = false;
            myDelegate = new AddConsoleItem(addConsoleItemMethod);

            try
            {
                if (File.Exists("readme.txt") == false)
                {
                    createReadme();
                }

                if (File.Exists("config.txt") == false)
                {
                    consoleMessage("config.txt - отсутствует");
                    saveConfigFile();
                }
                else
                {
                    readConfigFile();
                }

                if (File.Exists("agent.jar") == false)
                {
                    consoleMessage("Файл agent.jar - отсутствует");
                }

                if (File.Exists("slave-agent.jnlp") == false)
                {
                    consoleMessage("Файл slave-agent.jnlp - отсутствует");
                }
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                this.Invoke(this.myDelegate, new object[] { e.Data.ToString() });
            }
            catch (Exception ex)
            {

            }
        }
        void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                this.Invoke(this.myDelegate, new object[] { e.Data.ToString() });
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (P != null)
                {
                    if (P.HasExited == false)
                    {
                        P.Kill();
                    }
                }

            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = CancelClose;
            if (CancelClose == true) this.Visible = false;
        }

        private void consoleMessage(String message)
        {
            consoleRichTextBox.Text = consoleRichTextBox.Text + message + Environment.NewLine;
            consoleRichTextBox.Select(consoleRichTextBox.Text.Length, consoleRichTextBox.Text.Length);
            consoleRichTextBox.ScrollToCaret();
        }

        private void выключитьАгентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelClose = false;
            Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void выключитьАгентаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CancelClose = false;
            Close();
        }

        private void saveConfigFile()
        {
            try
            {
                StreamWriter writer;
                writer = new StreamWriter("config.txt", false, new UTF8Encoding(false));
                //    writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                //    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                //    writer = new StreamWriter(filename, false, Encoding.Default);

                string config = textBox1.Text + Environment.NewLine;
                config += textBox2.Text + Environment.NewLine;
                config += textBox3.Text + Environment.NewLine;
                config += textBox4.Text + Environment.NewLine;
                config += textBox5.Text + Environment.NewLine;
                config += textBox6.Text;

                writer.Write(config);
                writer.Close();
                consoleMessage("Сохранён файл config.txt");
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void readConfigFile()
        {
            try
            {
                StreamReader reader;
                reader = new StreamReader("config.txt", new UTF8Encoding(false));
                //reader = new StreamReader(filename, new UTF8Encoding(true));
                //reader = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                //reader = new StreamReader(filename, Encoding.Default);

                for (int i = 1; i <= 6; i++)
                {
                    if (i == 1) textBox1.Text = reader.ReadLine();
                    if (i == 2) textBox2.Text = reader.ReadLine();
                    if (i == 3) textBox3.Text = reader.ReadLine();
                    if (i == 4) textBox4.Text = reader.ReadLine();
                    if (i == 5) textBox5.Text = reader.ReadLine();
                    if (i == 6) textBox6.Text = reader.ReadLine();
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }

        }

        private void createReadme()
        {
            try
            {
                StreamWriter writer;
                //    writer = new StreamWriter(filename, false, Encoding.Default);
                //    writer = new StreamWriter("readme.txt", false, new UTF8Encoding(false));
                writer = new StreamWriter("readme.txt", false, new UTF8Encoding(true));
                //    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                //    writer = new StreamWriter(filename, false, Encoding.Default);

                string readme = @"
СОЗДАНИЕ АГЕНТА
===============================================================
1. Открыть ""Настройки Jenkins"" http://localhost:8080/manage
2. Перейти в ""Глобальные настройки безопасности"" http://localhost:8080/configureSecurity/
3. В разделе Agents включить флаг TCP port for JNLP agents в состояние Случайный/Random
4. Сохранить настройки
5. Вернуться в ""Настройки Jenkins"" http://localhost:8080/manage
6. Перейти в ""Управление средами сборки"" http://localhost:8080/computer/
7. Нажать на кнопку ""Новый узел"" (New Node)
	7.1 наименование (Name) (например: agent)
	7.2 включить флаг Permanent Agent
	7.3 нажать ОК
	7.4 Количество процессов-исполнителей (of executors): 2
	7.5 Корень удаленной папки (Remote root directory): C:\Jenkins\workspace
	7.6 Метки (Label): agent
	7.7 Использование (Usage): Only build jobs with label expressions matching this node
	7.8 Способ запуска (Launch method): Launch agent by connecting it to the master (все параметры оставить по умолчанию)
	7.9 Доступность (Availability): Keep this agent online as much as possible
	7.10 Node Properties: не включать флаги
	7.11 нажать кнопку Save

8. Вернуться в ""Управление средами сборки"" http://localhost:8080/computer/
9. В таблице нажать на proxy
===============================================================
ЗАПУСК АГЕНТА
===============================================================
10. Jenkins предлогает несколько способов запуска агента
	10.1 способ №1: выполнить команду
		javaws http://localhost:8080/computer/proxy/slave-agent.jnlp
		
	10.2 способ №2: скачать файл agent.jar по ссылке http://localhost:8080/jnlpJars/agent.jar	и выполнить команду
		java -jar agent.jar -jnlpUrl http://localhost:8080/computer/proxy/slave-agent.jnlp -secret 0000000000000000000000000000000000000000000000000000000000000000 -workDir ""C:\Jenkins\workspace""

11. Запуск агента с помощью JenkinsAgentRun
	11.1 создайне папку, например C:\Jenkins\ и поместите в папку файлы:
		agent.jar
		slave-agent.jnlp
		JenkinsAgentRun.exe
	11.2 запустите программу JenkinsAgentRun.exe она создаст файл config.txt
	11.3 введите в поля ранее полученные данные (пункт 10) и нажмите в меню ""Файл"" пункт ""Сохранить настройки""
		- путь к приложению (java.exe)
		- параметры языка (-Dfile.encoding=UTF8) для поддержки русского языка
		- путь к файлу agent.jar (http://localhost:8080/jnlpJars/agent.jar)
		- путь к файлу slave-agent.jnlp (http://localhost:8080/computer/proxy/slave-agent.jnlp)
		- укажите secret (0000000000000000000000000000000000000000000000000000000000000000)
		- укажите рабочую папку workDir (C:\Jenkins\workspace)
	11.4 выполните запуск агента с помощью менб ""Действие"" пункт ""Запустить агента""
	Если все поля были правильно заполнены агент будет запущен
===============================================================
НАСТРОЙКА ЗАДАЧИ
===============================================================
12. Настройка Job для работы с агентом
	12.1 включить параметр ""Ограничить лейблы сборщиков, которые могут исполнять данную задачу""
	12.2 в поле Label Expression ввести метку agent
";

                writer.Write(readme);
                writer.Close();
                consoleMessage("Создан файл readme.txt");
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void запуститьАгентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("config.txt") == false)
                {
                    consoleMessage("config.txt - отсутствует");
                    saveConfigFile();
                    return;
                }
                else
                {
                    readConfigFile();
                }

                if (File.Exists("agent.jar") == false)
                {
                    consoleMessage("Файл agent.jar - отсутствует");
                    return;
                }

                if (File.Exists("slave-agent.jnlp") == false)
                {
                    consoleMessage("Файл slave-agent.jnlp - отсутствует");
                    return;
                }

                consoleMessage("Запуск агента");
                string arguments = textBox2.Text + " -jar " + textBox3.Text + " -jnlpUrl " + textBox4.Text + " -secret " + textBox5.Text + " -workDir \"" + textBox6.Text + "\"";
                toolStripStatusLabel1.Text = arguments;
                consoleMessage("Команда для закуска: " + textBox1.Text + " " + arguments);

                P = new Process();
                P.StartInfo.FileName = textBox1.Text;
                P.StartInfo.Arguments = arguments;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.RedirectStandardInput = true;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.CreateNoWindow = true;
                P.StartInfo.UseShellExecute = false;
                P.ErrorDataReceived += P_ErrorDataReceived;
                P.OutputDataReceived += P_OutputDataReceived;
                P.Start();
                P.BeginErrorReadLine();
                P.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void остановитьАгентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                consoleMessage("Остановка агента");

                if (P != null)
                {
                    if (P.HasExited == false)
                    {
                        P.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void сохранитьНастройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveConfigFile();
        }


        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (help == false)
            {
                FormHelp help = new FormHelp();
                help.FormClosed += Help_FormClosed;
                help.Shown += Help_Shown;
                help.Show();
            }
            
        }

        private void Help_Shown(object sender, EventArgs e)
        {
            help = true;
        }

        private void Help_FormClosed(object sender, FormClosedEventArgs e)
        {
            help = false;
        }

        private void createBatFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter writer;
                writer = new StreamWriter("jenkins_agent_run.bat", false, new UTF8Encoding(false));
                //    writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                //    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                //    writer = new StreamWriter(filename, false, Encoding.Default);

                string command = textBox1.Text + " " + textBox2.Text + " -jar " + textBox3.Text + " -jnlpUrl " + textBox4.Text + " -secret " + textBox5.Text + " -workDir \"" + textBox6.Text + "\"";
                writer.Write(command);
                writer.Close();
                consoleMessage("Создан файл jenkins_agent_run.bat");
                MessageBox.Show("Создан файл jenkins_agent_run.bat", "Сообщение");
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }
    }
}
