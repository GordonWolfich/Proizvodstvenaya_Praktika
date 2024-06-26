using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace PPraktika
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_parse(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Yellow;
            label2.Text = "Ожидание";
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Выберите JSON файл"
            };

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string jsonContent = File.ReadAllText(openFile.FileName);
                JObject jsonObject = JObject.Parse(jsonContent);

                listBox1.Items.Clear();

                label2.ForeColor = Color.Red;
                label2.Text = "Почти готово!";

                JArray namesArray = (JArray)jsonObject["names"];
                if (namesArray != null)
                {
                    foreach (var item in namesArray)
                    {
                        string firstName = item["firstName"]?.ToString();
                        string lastName = item["lastName"]?.ToString();
                        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                        {
                            listBox1.Items.Add($"{firstName} {lastName}");
                            label2.ForeColor = Color.LimeGreen;
                            label2.Text = "Готово!";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("В файле отсутствует массив 'names'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_clear(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void btn_exit(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AddItemToListBox(JToken item)
        {
            if (item["name"] != null)
            {
                listBox1.Items.Add(item["name"].ToString());
            }
            else
            {
                // Если свойства "name" нет, добавляем все свойства объекта
                foreach (var property in item.Children<JProperty>())
                {
                    listBox1.Items.Add($"{property.Name}: {property.Value}");
                }
            }
        }
            private void btn_save(object sender, EventArgs e)
        {
            if(listBox1.Items.Count == 0)
            {
                MessageBox.Show("Извините, я ничего не могу сохранить :(");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Сохраните результат"
            };
            if(saveFile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFile.FileName))
                {
                    foreach(var item in listBox1.Items)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }
                MessageBox.Show("Результаты успешно сохранены.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                
            
        }


    }
}
