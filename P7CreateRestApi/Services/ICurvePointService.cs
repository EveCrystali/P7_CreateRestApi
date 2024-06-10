using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Services
{
    public interface ICurvePointService
    {
        Task<CurvePoint> SaveCurvePoint(CurvePoint curvePoint);

        Task<CurvePoint?> GetCurvePointByCurveId(byte? CurveId);

        bool ValidateCurvePoint(CurvePoint curvePoint);

        Task <List<CurvePoint>> GetAllCurvePoints();

        Task<List<BidList>> GetAllBidLists();
    }
}