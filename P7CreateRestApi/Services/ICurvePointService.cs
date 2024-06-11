using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Services
{
    public interface ICurvePointService
    {
        Task<CurvePoint> SaveCurvePoint(CurvePoint curvePoint);

        Task<CurvePoint> UpdateCurvePoint(CurvePoint existingCurvePoint);

        Task<int> DeleteCurvePoint(CurvePoint curvePoint);

        Task<CurvePoint?> GetCurvePointByCurveId(byte? CurveId);

        Task<CurvePoint?> GetCurvePointById(int id);

        bool CurvePointExistsByCurveId(byte? CurveId);

        bool CurvePointExistsById(int? Id);

        bool ValidateCurvePoint(CurvePoint curvePoint);

        Task <List<CurvePoint>> GetAllCurvePoints();

        Task<List<BidList>> GetAllBidLists();
    }
}