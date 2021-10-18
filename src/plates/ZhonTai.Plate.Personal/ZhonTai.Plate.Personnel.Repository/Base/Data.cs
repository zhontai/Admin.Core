using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Personnel.Domain;

namespace ZhonTai.Plate.Admin.Repository
{
    /// <summary>
    /// 数据
    /// </summary>
    public partial class Data
    {
        #region Personnel
        public PositionEntity[] Positions { get; set; }
        public OrganizationEntity[] OrganizationTree { get; set; }
        public EmployeeEntity[] Employees { get; set; } 
        #endregion
    }
}