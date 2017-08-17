using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Properties;

namespace namespaceToDO
{
    public partial class ToDO : Form
    {
        StoreInDatabase db = new StoreInDatabase();
        
        TextBox[] textBox = new TextBox[10];
        Button[] button = new Button[10];
        CheckBox[] checkBox = new CheckBox[10];

        public ToDO()
        {
            InitializeComponent();

            createCheckBoxes();
            createTextBoxes();
            createButtonBoxes();

            InitialSetup();
            restoreSetup();
        }

        public void restoreSetup()
        {
            String [] singleRow = new String[3];
            for(int i=0;i<10;i++)
            {
                singleRow = db.Retrieve(i);
                if(singleRow[0]!="") 
                {
                    this.textBox[i].Text = singleRow[0];
                    if (singleRow[1] == "" || Int32.Parse(singleRow[1]) == 0)
                    {
                        this.textBox[i].ReadOnly = false;
                        this.button[i].BackgroundImage = Resources.unlock;
                        this.button[i].BackgroundImage.Tag = "Unlocked";
                    }
                    else
                    {
                        this.textBox[i].ReadOnly = true;
                        this.button[i].BackgroundImage = Resources._lock;
                        this.button[i].BackgroundImage.Tag = "locked";
                    }
                    if (singleRow[2] == "" || Int32.Parse(singleRow[2]) == 0)
                        this.checkBox[i].Checked = false;
                    else
                        this.checkBox[i].Checked = true;

                    this.textBox[i].Visible = this.button[i].Visible = this.checkBox[i].Visible = true;
                }
            }
        }

        public void InitialSetup()
        {
            for (int i = 0; i < 10;i++)
            {
                if(i!=0)
                    this.textBox[i].Visible =  this.button[i].Visible = this.checkBox[i].Visible = false;
                if(i!=9)
                    this.textBox[i].KeyDown += new KeyEventHandler(enter_Press);
                this.textBox[i].LostFocus += new EventHandler(textBox_LostFocus);
                this.button[i].BackgroundImage = Resources.unlock;
                this.button[i].BackgroundImage.Tag = "Unlocked";
                this.button[i].Click += new System.EventHandler(button_Click);
            }
        }

        public void createTextBoxes()
        {
            for(int i = 0; i < 10; i++)
            {
                this.textBox[i] = new TextBox();
                this.textBox[i].BackColor = System.Drawing.Color.Snow;
                this.textBox[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.textBox[i].Dock = System.Windows.Forms.DockStyle.Fill;
                this.textBox[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.textBox[i].Location = new System.Drawing.Point(1, 1);
                this.textBox[i].Margin = new System.Windows.Forms.Padding(1);
                this.textBox[i].Name = "textBox" + i;
                this.textBox[i].Size = new System.Drawing.Size(315, 31);
                this.textBox[i].TabIndex = i;
                this.textBox[i].Visible = true;
                this.tableLayoutPanel1.Controls.Add(this.textBox[i], 0, i);
            }
        }

        public void createButtonBoxes()
        {
            for(int i = 0; i < 10; i++)
            {
                this.button[i] = new Button();
                this.button[i].BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.button[i].Location = new System.Drawing.Point(320, 296);
                this.button[i].Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
                this.button[i].Name = "button" + i;
                this.button[i].Size = new System.Drawing.Size(20, 20);
                this.button[i].TabIndex = 29;
                this.button[i].TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
                this.button[i].UseVisualStyleBackColor = true;
                this.tableLayoutPanel1.Controls.Add(this.button[i], 1, i);
            }
        }

        public void createCheckBoxes()
        {
            for (int i = 0; i < 10; i++)
            {
                this.checkBox[i] = new CheckBox();
                this.checkBox[i].AutoSize = true;
                this.checkBox[i].Location = new System.Drawing.Point(353, 228);
                this.checkBox[i].Margin = new System.Windows.Forms.Padding(10, 12, 3, 3);
                this.checkBox[i].Name = "checkBox" + i;
                this.checkBox[i].Size = new System.Drawing.Size(15, 14);
                this.checkBox[i].TabIndex = i;
                this.checkBox[i].UseVisualStyleBackColor = true;
                this.checkBox[i].Visible = true;
                this.tableLayoutPanel1.Controls.Add(this.checkBox[i], 2, i);
            }
        }

        public void textBox_LostFocus(Object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectionStart = 0;
        }

        public void button_Click(Object sender, EventArgs e)
        {
            Button button = (Button)sender;
            String button_TAG = (String)button.BackgroundImage.Tag;
            if (button_TAG == "Unlocked")
            {
                button.BackgroundImage = Resources._lock;
                button.BackgroundImage.Tag = "locked";
                int index = Int32.Parse(button.Name.ToString().Substring(button.Name.ToString().Length - 1));
                this.textBox[index].ReadOnly = true;
            }
            else if (button_TAG == "locked")
            {
                button.BackgroundImage = Resources.unlock;
                button.BackgroundImage.Tag = "Unlocked";
                int index = Int32.Parse(button.Name.ToString().Substring(button.Name.ToString().Length - 1));
                this.textBox[index].ReadOnly = false;
            }
        }

        public void enter_Press(Object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                TextBox currentTextBox = (TextBox)sender;
                currentTextBox.Focus();
                currentTextBox.SelectionStart = 0;
                currentTextBox.Visible = true;

                int index = Int32.Parse(currentTextBox.Name.ToString().Substring(currentTextBox.Name.ToString().Length - 1));
                
                // ADD to database
                db.Insert(index, currentTextBox.Text, (String)this.button[index].BackgroundImage.Tag, this.checkBox[index].Checked);

                
                this.textBox[index + 1].Visible = true;
                this.textBox[index + 1].Focus();
                this.checkBox[index + 1].Visible = true;
                this.button[index + 1].Visible = true;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
