using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOM.Model
{
    class ErrorCodeModel
    {
        private int _id;
        /// <summary>
        /// errorcode的编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _timeModel;
        /// <summary>
        /// errorCode的timemodel
        /// </summary>
        public string TimeModel
        {
            get { return _timeModel; }
            set { _timeModel = value; }
        }
        private string _errorCode;
        /// <summary>
        /// errorcode
        /// </summary>
        public string ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }
        private string _descript;
        /// <summary>
        /// errorcode描述
        /// </summary>
        public string Descript
        {
            get { return _descript; }
            set { _descript = value; }
        }
        private string _color;
        /// <summary>
        /// 报错时使用的颜色
        /// </summary>
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private string _notes;
        /// <summary>
        /// errorcode的记录
        /// </summary>
        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
    }
}
