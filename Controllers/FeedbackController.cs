using bshbbackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using feedback = bshbbackend.Models.feedback;

namespace bshbbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly BshbDbContext _context;

        public FeedbackController(BshbDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SubmitFeedback([FromBody] feedback feedback)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();

                return Ok("Feedback submitted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error submitting feedback: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetFeedback()
        {
            try
            {
                var feedbackList = _context.Feedbacks.ToList();
                return Ok(feedbackList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving feedback: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetFeedbackById(int id)
        {
            try
            {
                var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
                if (feedback == null)
                {
                    return NotFound("Feedback not found");
                }
                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving feedback: " + ex.Message);
            }
        }
    }
}
