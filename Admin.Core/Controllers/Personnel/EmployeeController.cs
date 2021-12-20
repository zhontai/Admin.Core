using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Service.Personnel.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Admin.Core.Model.Personnel;
using Admin.Core.Service.Personnel.Employee.Input;

namespace Admin.Core.Controllers.Personnel
{
    /// <summary>
    /// 员工管理
    /// </summary>
    public class EmployeeController : AreaController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// 查询单条员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(long id)
        {
            return await _employeeService.GetAsync(id);
        }

        /// <summary>
        /// 查询分页员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[ResponseCache(Duration = 60)]
        public async Task<IResponseOutput> GetPage(PageInput<EmployeeEntity> input)
        {
            return await _employeeService.PageAsync(input);
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(EmployeeAddInput input)
        {
            return await _employeeService.AddAsync(input);
        }

        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(EmployeeUpdateInput input)
        {
            return await _employeeService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(long id)
        {
            return await _employeeService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除员工
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(long[] ids)
        {
            return await _employeeService.BatchSoftDeleteAsync(ids);
        }
    }
}