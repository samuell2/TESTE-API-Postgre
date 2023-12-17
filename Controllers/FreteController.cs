using WebApplication1.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using WebApplication1.Services;
public class FreteController : Controller
{

    public class ResponseFreteCal
    {
        public double ValorFrete { get; set; }
        public double ValorPago { get; set; }

        public string Nome {get; set;}

        public ResponseFreteCal(double valor)
        {
            this.ValorFrete = valor;
        }
    }

  


    [HttpPost]
    [Route("CalculoFrete")]
    public IActionResult sendRequest(Frete data)
    {
        try
        {
            
            FreteAdivisor sendRequest = new FreteAdivisor();
            var Results = sendRequest.sendRequest(data);
            return Ok(new { success = true, result = Results });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, mensagem = ex.Message });
        }
    }
    public ResponseFreteCal Calculo(Frete frete)
    {
        ResponseFreteCal calculoTotal = new ResponseFreteCal(CalculoPeso(frete) + TaxaEstado(frete) + TaxaQuebravel(frete));
        return calculoTotal;  
    }
    private double CalculoPeso (Frete frete)
    {
        if (frete.Peso >= 0 && frete.Peso <= 10) return 0.8 * frete.Peso;   
        else if (frete.Peso >= 10.1 && frete.Peso <= 20) return 0.96 * frete.Peso;
        else return 2.1 * frete.Peso;
    }
    private double TaxaEstado (Frete frete)
    {
        if (frete.UF == "MG" || frete.UF == "SP" || frete.UF == "ES") return frete.Peso * 5;
        else if (frete.UF == "PR" || frete.UF == "SC" || frete.UF == "RS") return frete.Peso * 15;
        else if (frete.UF == "AC" || frete.UF == "AM" || frete.UF == "RO" || frete.UF == "RR" || frete.UF == "AP" || frete.UF == "PA" || frete.UF == "TO") return frete.Peso * 50;
        else return frete.Peso * 35;
    }

    private double TaxaQuebravel(Frete frete)
    {
        if (frete.Quebravel) return CalculoPeso(frete) * 0.25;
        else return 0;
    }
}
