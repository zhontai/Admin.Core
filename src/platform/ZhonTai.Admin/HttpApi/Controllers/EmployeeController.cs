using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Admin.Services.Employee.Input;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ZhonTai.Admin.HttpApi.Controllers
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
        /// 查询员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResultOutput> Get([BindRequired] long id)
        {
            return await _employeeService.GetAsync(id);
        }

        /// <summary>
        /// 查询员工列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[ResponseCache(Duration = 60)]
        public async Task<IResultOutput> GetPage(PageInput input)
        {
            return await _employeeService.GetPageAsync(input);
        }

        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> Add(EmployeeAddInput input)
        {
            return await _employeeService.AddAsync(input);
        }

        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> Update(EmployeeUpdateInput input)
        {
            return await _employeeService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResultOutput> SoftDelete(long id)
        {
            return await _employeeService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除员工
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResultOutput> BatchSoftDelete(long[] ids)
        {
            return await _employeeService.BatchSoftDeleteAsync(ids);
        }
    }
}