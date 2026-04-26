using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using G2CarRentalCustomers.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using CarRentalPlatform.Models;

[ApiController]
[Route("api/[controller]")]
public class G2CustomersApiController : ControllerBase
{
	private readonly G2CustomerProfileContext _context;

	public G2CustomersApiController(G2CustomerProfileContext context)
	{
		_context = context;
	}

	// GET: api/CustomersApi
	[HttpGet]
	public async Task<ActionResult<IEnumerable<G2Customer>>> GetCustomers()
	{
		return await _context.Customers.ToListAsync();
	}

	// GET: api/CustomersApi/5
	[HttpGet("{id}")]
	public async Task<ActionResult<G2Customer>> GetCustomer(int id)
	{
		var customer = await _context.Customers.FindAsync(id);

		if (customer == null)
			return NotFound();

		return customer;
	}

	// POST: api/CustomersApi
	[HttpPost]
	public async Task<ActionResult<G2Customer>> PostCustomer(G2Customer g2Customer)
	{

		g2Customer.Id = 0;
		_context.Customers.Add(g2Customer);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetCustomer),
			new { id = g2Customer.Id }, g2Customer);
	}

	// PUT: api/CustomersApi/5
	[HttpPut("{id}")]
	public async Task<IActionResult> PutCustomer(int id, G2Customer g2Customer)
	{
		if (id != g2Customer.Id)
			return BadRequest();

		_context.Entry(g2Customer).State = EntityState.Modified;
		await _context.SaveChangesAsync();

		return NoContent();
	}

	// DELETE: api/CustomersApi/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteCustomer(int id)
	{
		var customer = await _context.Customers.FindAsync(id);

		if (customer == null)
			return NotFound();

		_context.Customers.Remove(customer);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}


