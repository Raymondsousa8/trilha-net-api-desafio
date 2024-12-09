using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Buscar o Id no banco utilizando o Entity Framework
            var tarefa = _context.Tarefas.Find(id);

            // Validar o tipo de retorno 
            if(tarefa == null)
            {
                //Se não encontrar a tarefa, retornar NotFound
                return NotFound();
            }
                else
                {
                    // caso contrário retornar OK com a tarefa encontrada
                    return Ok(tarefa);
                }
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Buscar todas as tarefas no banco utilizando o Entity Framework
            var tarefas = _context.Tarefas.ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Buscar  as tarefas no banco utilizando o Entity Framework, que contenha o titulo recebido por parâmetro
            var tarefas = _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToList();

            // Validar se encontrou alguma tarefa
            if (tarefas == null || !tarefas.Any())
            {
                // Se não encontrar nenhuma tarefa, retornar NotFound
                return NotFound();
            }
            else
            {
                // Caso contrário, retornar OK com a lista de tarefas encontradas
                return Ok(tarefas);
            }
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Buscar  as tarefas no banco utilizando o Entity Framework, que contenha o status recebido por parâmetro
            var tarefas = _context.Tarefas.Where(x => x.Status == status).ToList();

            // Validar se encontrou alguma tarefa
            if(tarefas == null || !tarefas.Any())
            {
                // Se não encontrar nenhuma tarefa, retornar NotFound
                return NotFound();
            }
            else
            {
                // Caso contrário, retornar OK com a lista de tarefas encontradas
                return Ok(tarefas);
            }
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adicionar a tarefa recebida no Entity Framework  
            _context.Tarefas.Add(tarefa);

            //salvar as mudanças no banco de dados
            _context.SaveChanges();

            // Retornar CreatedAtAction com a URI para o recurso criado
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualiza as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;
            // Atualizar a variável tarefaBanco no Entity Framework e salvar as mudanças
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            // Retornar OK com a tarefa atualizada
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // Remover a tarefa encontrada através do Entity Framework  
            _context.Tarefas.Remove(tarefaBanco);

            // Salvar as mudanças no banco de dados
            _context.SaveChanges();

            // Retornar NoContent para indicar que a tarefa foi removida
            return NoContent();
        }
    }
}
