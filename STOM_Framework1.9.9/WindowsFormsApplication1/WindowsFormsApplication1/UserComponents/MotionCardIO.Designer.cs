namespace STOM.UserComponents
{
    partial class MotionCardIO
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
            this.lbl_IOName = new System.Windows.Forms.Label();
            this.txtBox_IOPoint = new System.Windows.Forms.TextBox();
            this.panel_IOStatus = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbl_IOName
            // 
            this.lbl_IOName.Location = new System.Drawing.Point(5, 0);
            this.lbl_IOName.Name = "lbl_IOName";
            this.lbl_IOName.Size = new System.Drawing.Size(81, 23);
            this.lbl_IOName.TabIndex = 0;
            this.lbl_IOName.Text = "Name";
            this.lbl_IOName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBox_IOPoint
            // 
            this.txtBox_IOPoint.Location = new System.Drawing.Point(92, 1);
            this.txtBox_IOPoint.Name = "txtBox_IOPoint";
            this.txtBox_IOPoint.Size = new System.Drawing.Size(49, 21);
            this.txtBox_IOPoint.TabIndex = 1;
            this.txtBox_IOPoint.TextChanged += new System.EventHandler(this.txtBox_IOPoint_TextChanged);
            // 
            // panel_IOStatus
            // 
            this.panel_IOStatus.BackColor = System.Drawing.Color.Lime;
            this.panel_IOStatus.Location = new System.Drawing.Point(145, 0);
            this.panel_IOStatus.Name = "panel_IOStatus";
            this.panel_IOStatus.Size = new System.Drawing.Size(26, 23);
            this.panel_IOStatus.TabIndex = 2;
            this.panel_IOStatus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_IOStatus_MouseClick);
            // 
            // MotionCardIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_IOStatus);
            this.Controls.Add(this.txtBox_IOPoint);
            this.Controls.Add(this.lbl_IOName);
            this.Name = "MotionCardIO";
            this.Size = new System.Drawing.Size(172, 24);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_IOName;
        private System.Windows.Forms.TextBox txtBox_IOPoint;
        private System.Windows.Forms.Panel panel_IOStatus;
    }
}
