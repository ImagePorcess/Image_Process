namespace STOM.UserComponents
{
    partial class MotionCardAxis
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBox_AxisNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBox_MinSpeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBox_MaxSpeed = new System.Windows.Forms.TextBox();
            this.txtBox_AxisPos = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel_EL_POSITIVE = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel_EL_NEGATIVE = new System.Windows.Forms.Panel();
            this.panel_ENABLE = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panel_ALARM = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_Home = new System.Windows.Forms.Button();
            this.txtBox_Move_Length = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_MoveNagetive = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBtn_PositionMove = new System.Windows.Forms.RadioButton();
            this.radioBtn_LengthMove = new System.Windows.Forms.RadioButton();
            this.radioBtn_ContinuousMove = new System.Windows.Forms.RadioButton();
            this.txtBox_MovePosition = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel_EMG = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panel_ORI = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_Move_Positive = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "当前位置:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "轴号:";
            // 
            // txtBox_AxisNum
            // 
            this.txtBox_AxisNum.Location = new System.Drawing.Point(64, 17);
            this.txtBox_AxisNum.Name = "txtBox_AxisNum";
            this.txtBox_AxisNum.Size = new System.Drawing.Size(83, 21);
            this.txtBox_AxisNum.TabIndex = 1;
            this.txtBox_AxisNum.TextChanged += new System.EventHandler(this.txtBox_AxisNum_TextChanged);
            this.txtBox_AxisNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_AxisNum_KeyPress);
            this.txtBox_AxisNum.Leave += new System.EventHandler(this.txtBox_AxisNum_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "初速度:";
            // 
            // txtBox_MinSpeed
            // 
            this.txtBox_MinSpeed.Location = new System.Drawing.Point(64, 52);
            this.txtBox_MinSpeed.Name = "txtBox_MinSpeed";
            this.txtBox_MinSpeed.Size = new System.Drawing.Size(83, 21);
            this.txtBox_MinSpeed.TabIndex = 3;
            this.txtBox_MinSpeed.TextChanged += new System.EventHandler(this.txtBox_MinSpeed_TextChanged);
            this.txtBox_MinSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_MinSpeed_KeyPress);
            this.txtBox_MinSpeed.Leave += new System.EventHandler(this.txtBox_MinSpeed_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "驱动速度:";
            // 
            // txtBox_MaxSpeed
            // 
            this.txtBox_MaxSpeed.Location = new System.Drawing.Point(64, 87);
            this.txtBox_MaxSpeed.Name = "txtBox_MaxSpeed";
            this.txtBox_MaxSpeed.Size = new System.Drawing.Size(83, 21);
            this.txtBox_MaxSpeed.TabIndex = 5;
            this.txtBox_MaxSpeed.TextChanged += new System.EventHandler(this.txtBox_MaxSpeed_TextChanged);
            this.txtBox_MaxSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_MaxSpeed_KeyPress);
            this.txtBox_MaxSpeed.Leave += new System.EventHandler(this.txtBox_MaxSpeed_Leave);
            // 
            // txtBox_AxisPos
            // 
            this.txtBox_AxisPos.Location = new System.Drawing.Point(64, 122);
            this.txtBox_AxisPos.Name = "txtBox_AxisPos";
            this.txtBox_AxisPos.ReadOnly = true;
            this.txtBox_AxisPos.Size = new System.Drawing.Size(83, 21);
            this.txtBox_AxisPos.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(155, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "正限位:";
            // 
            // panel_EL_POSITIVE
            // 
            this.panel_EL_POSITIVE.BackColor = System.Drawing.Color.Lime;
            this.panel_EL_POSITIVE.Location = new System.Drawing.Point(206, 17);
            this.panel_EL_POSITIVE.Name = "panel_EL_POSITIVE";
            this.panel_EL_POSITIVE.Size = new System.Drawing.Size(19, 21);
            this.panel_EL_POSITIVE.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(155, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "负限位:";
            // 
            // panel_EL_NEGATIVE
            // 
            this.panel_EL_NEGATIVE.BackColor = System.Drawing.Color.Lime;
            this.panel_EL_NEGATIVE.Location = new System.Drawing.Point(206, 41);
            this.panel_EL_NEGATIVE.Name = "panel_EL_NEGATIVE";
            this.panel_EL_NEGATIVE.Size = new System.Drawing.Size(19, 21);
            this.panel_EL_NEGATIVE.TabIndex = 11;
            // 
            // panel_ENABLE
            // 
            this.panel_ENABLE.BackColor = System.Drawing.Color.Lime;
            this.panel_ENABLE.Location = new System.Drawing.Point(206, 139);
            this.panel_ENABLE.Name = "panel_ENABLE";
            this.panel_ENABLE.Size = new System.Drawing.Size(19, 21);
            this.panel_ENABLE.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(155, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "使能:";
            // 
            // panel_ALARM
            // 
            this.panel_ALARM.BackColor = System.Drawing.Color.Lime;
            this.panel_ALARM.Location = new System.Drawing.Point(206, 90);
            this.panel_ALARM.Name = "panel_ALARM";
            this.panel_ALARM.Size = new System.Drawing.Size(19, 21);
            this.panel_ALARM.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(155, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "报警:";
            // 
            // btn_Home
            // 
            this.btn_Home.Location = new System.Drawing.Point(242, 188);
            this.btn_Home.Name = "btn_Home";
            this.btn_Home.Size = new System.Drawing.Size(57, 31);
            this.btn_Home.TabIndex = 16;
            this.btn_Home.Text = "回原点";
            this.btn_Home.UseVisualStyleBackColor = true;
            this.btn_Home.Click += new System.EventHandler(this.btn_Home_Click);
            // 
            // txtBox_Move_Length
            // 
            this.txtBox_Move_Length.Location = new System.Drawing.Point(64, 157);
            this.txtBox_Move_Length.Name = "txtBox_Move_Length";
            this.txtBox_Move_Length.Size = new System.Drawing.Size(83, 21);
            this.txtBox_Move_Length.TabIndex = 19;
            this.txtBox_Move_Length.TextChanged += new System.EventHandler(this.txtBox_Move_Length_TextChanged);
            this.txtBox_Move_Length.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_Move_Length_KeyPress);
            this.txtBox_Move_Length.Leave += new System.EventHandler(this.txtBox_Move_Length_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-1, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "移动距离:";
            // 
            // btn_MoveNagetive
            // 
            this.btn_MoveNagetive.Image = global::STOM.Properties.Resources.down48px;
            this.btn_MoveNagetive.Location = new System.Drawing.Point(242, 105);
            this.btn_MoveNagetive.Name = "btn_MoveNagetive";
            this.btn_MoveNagetive.Size = new System.Drawing.Size(57, 55);
            this.btn_MoveNagetive.TabIndex = 22;
            this.btn_MoveNagetive.UseVisualStyleBackColor = true;
            this.btn_MoveNagetive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_MoveNagetive_KeyDown);
            this.btn_MoveNagetive.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_MoveNagetive_KeyUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBtn_PositionMove);
            this.groupBox1.Controls.Add(this.radioBtn_LengthMove);
            this.groupBox1.Controls.Add(this.radioBtn_ContinuousMove);
            this.groupBox1.Controls.Add(this.txtBox_MovePosition);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.panel_EMG);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.panel_ORI);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btn_MoveNagetive);
            this.groupBox1.Controls.Add(this.btn_Move_Positive);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBox_Move_Length);
            this.groupBox1.Controls.Add(this.btn_Home);
            this.groupBox1.Controls.Add(this.panel_ENABLE);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.panel_ALARM);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.panel_EL_NEGATIVE);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.panel_EL_POSITIVE);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBox_AxisPos);
            this.groupBox1.Controls.Add(this.txtBox_MaxSpeed);
            this.groupBox1.Controls.Add(this.txtBox_MinSpeed);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtBox_AxisNum);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(319, 221);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "运动轴设置";
            // 
            // radioBtn_PositionMove
            // 
            this.radioBtn_PositionMove.AutoSize = true;
            this.radioBtn_PositionMove.Location = new System.Drawing.Point(242, 168);
            this.radioBtn_PositionMove.Name = "radioBtn_PositionMove";
            this.radioBtn_PositionMove.Size = new System.Drawing.Size(71, 16);
            this.radioBtn_PositionMove.TabIndex = 33;
            this.radioBtn_PositionMove.TabStop = true;
            this.radioBtn_PositionMove.Text = "点位运动";
            this.radioBtn_PositionMove.UseVisualStyleBackColor = true;
            // 
            // radioBtn_LengthMove
            // 
            this.radioBtn_LengthMove.AutoSize = true;
            this.radioBtn_LengthMove.Location = new System.Drawing.Point(157, 196);
            this.radioBtn_LengthMove.Name = "radioBtn_LengthMove";
            this.radioBtn_LengthMove.Size = new System.Drawing.Size(71, 16);
            this.radioBtn_LengthMove.TabIndex = 32;
            this.radioBtn_LengthMove.TabStop = true;
            this.radioBtn_LengthMove.Text = "定长运动";
            this.radioBtn_LengthMove.UseVisualStyleBackColor = true;
            // 
            // radioBtn_ContinuousMove
            // 
            this.radioBtn_ContinuousMove.AutoSize = true;
            this.radioBtn_ContinuousMove.Location = new System.Drawing.Point(157, 168);
            this.radioBtn_ContinuousMove.Name = "radioBtn_ContinuousMove";
            this.radioBtn_ContinuousMove.Size = new System.Drawing.Size(71, 16);
            this.radioBtn_ContinuousMove.TabIndex = 31;
            this.radioBtn_ContinuousMove.TabStop = true;
            this.radioBtn_ContinuousMove.Text = "连续运动";
            this.radioBtn_ContinuousMove.UseVisualStyleBackColor = true;
            // 
            // txtBox_MovePosition
            // 
            this.txtBox_MovePosition.Location = new System.Drawing.Point(64, 192);
            this.txtBox_MovePosition.Name = "txtBox_MovePosition";
            this.txtBox_MovePosition.Size = new System.Drawing.Size(83, 21);
            this.txtBox_MovePosition.TabIndex = 30;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(-1, 196);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 12);
            this.label12.TabIndex = 29;
            this.label12.Text = "定点点位:";
            // 
            // panel_EMG
            // 
            this.panel_EMG.BackColor = System.Drawing.Color.Lime;
            this.panel_EMG.Location = new System.Drawing.Point(206, 114);
            this.panel_EMG.Name = "panel_EMG";
            this.panel_EMG.Size = new System.Drawing.Size(19, 21);
            this.panel_EMG.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(155, 119);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 26;
            this.label11.Text = "急停:";
            // 
            // panel_ORI
            // 
            this.panel_ORI.BackColor = System.Drawing.Color.Lime;
            this.panel_ORI.Location = new System.Drawing.Point(206, 65);
            this.panel_ORI.Name = "panel_ORI";
            this.panel_ORI.Size = new System.Drawing.Size(19, 21);
            this.panel_ORI.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(155, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "原点:";
            // 
            // btn_Move_Positive
            // 
            this.btn_Move_Positive.Image = global::STOM.Properties.Resources.up48px;
            this.btn_Move_Positive.Location = new System.Drawing.Point(242, 17);
            this.btn_Move_Positive.Name = "btn_Move_Positive";
            this.btn_Move_Positive.Size = new System.Drawing.Size(57, 55);
            this.btn_Move_Positive.TabIndex = 21;
            this.btn_Move_Positive.UseVisualStyleBackColor = true;
            this.btn_Move_Positive.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_Move_Positive_KeyDown);
            this.btn_Move_Positive.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_Move_Positive_KeyUp);
            // 
            // MotionCardAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "MotionCardAxis";
            this.Size = new System.Drawing.Size(319, 221);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBox_AxisNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBox_MinSpeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBox_MaxSpeed;
        private System.Windows.Forms.TextBox txtBox_AxisPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_EL_POSITIVE;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel_EL_NEGATIVE;
        private System.Windows.Forms.Panel panel_ENABLE;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel_ALARM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_Home;
        private System.Windows.Forms.TextBox txtBox_Move_Length;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_Move_Positive;
        private System.Windows.Forms.Button btn_MoveNagetive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel_ORI;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel_EMG;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBox_MovePosition;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton radioBtn_PositionMove;
        private System.Windows.Forms.RadioButton radioBtn_LengthMove;
        private System.Windows.Forms.RadioButton radioBtn_ContinuousMove;
    }
}
