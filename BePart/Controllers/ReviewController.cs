using BePart.Data;
using Microsoft.AspNetCore.Mvc;
using SharedData;

namespace BePart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult GetAllReviews()
        {
            var reviews = _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        [HttpPost]
        public IActionResult AddReview([FromBody] Review review)
        {
            _reviewService.AddReview(review);
            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            var existingReview = _reviewService.GetReviewById(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            _reviewService.UpdateReview(review);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _reviewService.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }

            _reviewService.DeleteReview(id);
            return NoContent();
        }
    }
}
