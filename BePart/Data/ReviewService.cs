using SharedData;

namespace BePart.Data
{
    public interface IReviewService
    {
        List<Review> GetAllReviews();
        List<Review> GetAllReviewsByHardware(int id);
        Review GetReviewById(int id);
        void AddReview(Review review);
        void UpdateReview(Review review);
        void DeleteReview(int id);
    }

    public class ReviewService : IReviewService
    {
        private readonly List<Review> _reviews = new List<Review>();
        private readonly IHardwareService _hardwareService;

        public ReviewService(IHardwareService hardwareService)
        {
            _hardwareService = hardwareService;
            _reviews.Add(new Review { Id = 1, Title = "Good", Text = "Enough", Rating = 1, HardwareId = 1 });
        }

        public List<Review> GetAllReviews()
        {
            foreach (var item in _reviews)
            {
                item.Hardware = _hardwareService.GetHardwareById(item.HardwareId);
            };

            return _reviews;
        }

        public List<Review> GetAllReviewsByHardware(int id)
        {
            var hardwareReviews = _reviews.Where(x => x.HardwareId == id);
            foreach (var item in hardwareReviews)
            {
                item.Hardware = _hardwareService.GetHardwareById(item.HardwareId);
            };

            return hardwareReviews.ToList();
        }

        public Review GetReviewById(int id)
        {
            var review = _reviews.SingleOrDefault(c => c.Id == id);
            review.Hardware = _hardwareService.GetHardwareById(review.HardwareId);

            return review;
        }

        public void AddReview(Review review)
        {
            var lastReview = _reviews.Count() == 0 ? 0 : _reviews.Last().Id;
            lastReview += 1;
            review.Id = lastReview;
            _reviews.Add(review);
        }

        public void UpdateReview(Review review)
        {
            var index = _reviews.FindIndex(c => c.Id == review.Id);
            if (index != -1)
            {
                _reviews[index] = review;
            }
        }

        public void DeleteReview(int id)
        {
            _reviews.RemoveAll(c => c.Id == id);
        }
    }
}

