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

        public bool CurvePointExistsByCurveId(byte? CurveId)
        {
            return _context.CurvePoints.Any(e => e.CurveId == CurveId);
        }

        public bool CurvePointExistsById(int? Id)
        {
            return _context.CurvePoints.Any(e => e.Id == Id);
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

        public async Task<CurvePoint?> GetCurvePointById(int id)
        {
            return await _context.CurvePoints.FindAsync(id);
        }

        public async Task<CurvePoint> SaveCurvePoint(CurvePoint curvePoint)
        {
            _context.CurvePoints.Add(curvePoint);
            await _context.SaveChangesAsync();
            return curvePoint;
        }

        public async Task<CurvePoint> UpdateCurvePoint(CurvePoint existingCurvePoint)
        {
            _context.Entry(existingCurvePoint).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingCurvePoint;
        }

        public async Task<int> DeleteCurvePoint(CurvePoint curvePoint)
        {
            if (curvePoint != null)
            {
                _context.CurvePoints.Remove(curvePoint);
                await _context.SaveChangesAsync();
                return 0;
            }
            return -1;
        }

        public bool ValidateCurvePoint(CurvePoint curvePoint)
        {
            if (curvePoint == null)
            {
                return false;
            }

            if (curvePoint.Term < 0)
            {
                return false;
            }

            return true;
        }
    }
}