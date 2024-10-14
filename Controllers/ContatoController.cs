using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using ModuloAPI.Context;
using ModuloAPI.Entities;

namespace ModuloAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : ControllerBase
    {
        private readonly AgendaContext _context;

        public ContatoController(AgendaContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var contato = _context.Contatos.Find(id);
            if(contato == null)
            {
                return NotFound();            
            }
            return Ok(contato);
        }
        
        // Forma Pruta sem utilizar o metodo Where
        // [HttpGet("Return/{name}")]     
        // public IActionResult ObterNome(string name)
        // {
        //     var contatoBanco = _context.Contatos;
        //     List<Contato> contatosName = new List<Contato>();
        //     foreach(var contato in contatoBanco)
        //     { 
        //         if(contato.Nome.ToUpper() == name.ToUpper())
        //         {
        //             contatosName.Add(contato);
        //         }
        //     }
        //     if(contatoBanco == null || contatosName.Count < 1)
        //         return NotFound();
            
        //     return Ok(contatosName);
        // }

        [HttpGet("Return/{nome}")]
        public IActionResult ObterNome(string nome){
            var contatos = _context.Contatos.Where(x => x.Nome.Contains(nome));
            if(nome == null)
                return NotFound();
            if(contatos.Count() < 1)
                return NoContent();

            return Ok(contatos);
        }

        [HttpGet("ReturnAll")]
        public IActionResult ObterTodos()
        {
           var  contatoBanco = _context.Contatos;
           return Ok(contatoBanco);
        }
        
        [HttpPost]
        public IActionResult Create(Contato contato)
        {
            _context.Add(contato);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new {id = contato.Id},contato);
        }
       
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Contato contato)
        {
            var contatoBanco = _context.Contatos.Find(id);
            
            if(contato == null)
                return NotFound();

            contatoBanco.Nome = contato.Nome;
            contatoBanco.Telefone = contato.Telefone;
            contatoBanco.Ativo = contato.Ativo; 

            _context.Contatos.Update(contatoBanco);
            _context.SaveChanges();

            return Ok(contatoBanco);   
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var contatoBanco = _context.Contatos.Find(id);

            if(contatoBanco == null)
                return NotFound();

            _context.Contatos.Remove(contatoBanco);
            _context.SaveChanges();
            return  NoContent();
        }
    }
}