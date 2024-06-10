using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Services
{
    public class CurvePointService : ICurvePointService
    {
        private readonly LocalDbContext _context;

        public CurvePointService(LocalDbContext context)
        {
            _context = context;
        }

        public bool CurvePointExists(byte? CurveId)
        {
            return _context.CurvePoints.Any(e => e.CurveId == CurveId);
        }

        public async Task<List<BidList>> GetAllBidLists()
        {
            return await _context.BidLists.ToListAsync();
        }

        public async Task<List<CurvePoint>> GetAllCurvePoints()
        {
            return await _context.CurvePoints.ToListAsync();
        }

        public async Task<CurvePoint?> GetCurvePointByCurveId(byte? CurveId)
        {
            return await _context.CurvePoints.FindAsync(CurveId);
        }

        public async Task<CurvePoint> SaveCurvePoint(CurvePoint curvePoint)
        {
            _context.CurvePoints.Add(curvePoint);
            await _context.SaveChangesAsync();
            return curvePoint;
        }

        public bool ValidateCurvePoint(CurvePoint curvePoint)
        {
            if (curvePoint == null)
            {
                return false;
            }

            if (curvePoint.CurveId == null)
            {
                return false;
            }

            if (CurvePointExists(curvePoint.CurveId))
            {
                return false;
            }

            if (curvePoint.Term < 0)
            {
                return false;
            }

            if (curvePoint.CurvePointValue == null)
            {
                return false;
            }

            return true;
        }
    }
}