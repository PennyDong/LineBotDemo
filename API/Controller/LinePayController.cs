using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LinePayController : ControllerBase
    {
        private readonly LinePayService _linePayService;

        public LinePayController()
        {
            _linePayService = new LinePayService();
        }
        
        [HttpPost("Create")]
        public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto dto)
        {
            return await _linePayService.SendPaymentRequest(dto);
        }

        [HttpPost("Confirm")]
        public async Task<PaymentConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId,[FromQuery] string orderId,PaymentConfirmDto dto)
        {
            return await _linePayService.ConfirmPayment(transactionId,orderId,dto);
        }

        [HttpGet("Cancel")]
        public async void CancelTransaction([FromQuery] string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
        }
    }
}