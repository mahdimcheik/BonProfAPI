using BonProf.Models;
using Microsoft.EntityFrameworkCore;
using BonProf.Contexts;
using BonProf.Models;
using BonProf.Utilities;
using System.Security.Claims;

namespace BonProf.Services;

/// <summary>
/// Service pour la gestion des produits
/// </summary>
public class ProductService(MainContext context)
{
    /// <summary>
    /// R�cup�re tous les produits
    /// </summary>
    /// <returns>Liste des produits</returns>
    public async Task<Response<List<ProductDetails>>> GetAllProductsAsync()
    {
        try
        {
            var products = await context.Products
                .AsNoTracking()
                .Include(p => p.Cursus)
                .Where(p => p.ArchivedAt == null)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProductDetails(p))
                .ToListAsync();

            return new Response<List<ProductDetails>>
            {
                Status = 200,
                Message = "Produits r�cup�r�s avec succ�s",
                Data = products,
                Count = products.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<ProductDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des produits: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// R�cup�re un produit par son identifiant
    /// </summary>
    /// <param name="id">Identifiant du produit</param>
    /// <returns>Produit trouv�</returns>
    public async Task<Response<ProductDetails>> GetProductByIdAsync(Guid id)
    {
        try
        {
            var product = await context.Products
                .AsNoTracking()
                .Include(p => p.Cursus)
                .FirstOrDefaultAsync(p => p.Id == id && p.ArchivedAt == null);

            if (product == null)
            {
                return new Response<ProductDetails>
                {
                    Status = 404,
                    Message = "Produit non trouv�",
                    Data = null
                };
            }

            return new Response<ProductDetails>
            {
                Status = 200,
                Message = "Produit r�cup�r� avec succ�s",
                Data = new ProductDetails(product)
            };
        }
        catch (Exception ex)
        {
            return new Response<ProductDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration du produit: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// R�cup�re les produits par cursus
    /// </summary>
    /// <param name="cursusId">Identifiant du cursus</param>
    /// <returns>Liste des produits du cursus</returns>
    public async Task<Response<List<ProductDetails>>> GetProductsByCursusIdAsync(Guid cursusId)
    {
        try
        {
            var products = await context.Products
                .AsNoTracking()
                .Include(p => p.Cursus)
                .Where(p => p.CursusId == cursusId && p.ArchivedAt == null)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProductDetails(p))
                .ToListAsync();

            return new Response<List<ProductDetails>>
            {
                Status = 200,
                Message = "Produits du cursus r�cup�r�s avec succ�s",
                Data = products,
                Count = products.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<ProductDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des produits du cursus: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// R�cup�re les produits par teacherId
    /// </summary>
    /// <param name="teacherId">Identifiant du cursus</param>
    /// <returns>Liste des produits du cursus</returns>
    public async Task<Response<List<ProductDetails>>> GetProductsByTeacherIdAsync(Guid teacherId)
    {
        try
        {
            var products = await context.Products
                .AsNoTracking()
                .Include(p => p.Cursus)
                .Where(p => p.Cursus.TeacherId == teacherId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ProductDetails(p))
                .ToListAsync();

            return new Response<List<ProductDetails>>
            {
                Status = 200,
                Message = "Produits du cursus r�cup�r�s avec succ�s",
                Data = products,
                Count = products.Count
            };
        }
        catch (Exception ex)
        {
            return new Response<List<ProductDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des produits du cursus: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Cr�e un nouveau produit
    /// </summary>
    /// <param name="productDto">Donn�es du produit � cr�er</param>
    /// <returns>Produit cr��</returns>
    public async Task<Response<ProductDetails>> CreateProductAsync(ProductCreate productDto, ClaimsPrincipal User)
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(User, context);
            if(user is null)
            {
                return new Response<ProductDetails>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifi�",
                    Data = null
                };
            }

            var cursus = await context.Cursuses
                .FirstOrDefaultAsync(c => c.Id == productDto.CursusId && c.ArchivedAt == null && c.TeacherId == user.Id);

            if (cursus is null)
            {
                return new Response<ProductDetails>
                {
                    Status = 401,
                    Message = "Cours non existant",
                    Data = null
                };
            }

            var product = new Product(productDto);

            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Recharger avec les relations pour la r�ponse
            var createdProduct = await context.Products
                .Include(p => p.Cursus)
                .FirstAsync(p => p.Id == product.Id);

            return new Response<ProductDetails>
            {
                Status = 201,
                Message = "Produit cr�� avec succ�s",
                Data = new ProductDetails(createdProduct)
            };
        }
        catch (Exception ex)
        {
            return new Response<ProductDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la cr�ation du produit: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Met � jour un produit existant
    /// </summary>
    /// <param name="id">Identifiant du produit</param>
    /// <param name="productDto">Nouvelles donn�es du produit</param>
    /// <returns>Produit mis � jour</returns>
    public async Task<Response<ProductDetails>> UpdateProductAsync(ProductUpdate productDto)
    {
        try
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == productDto.Id && p.ArchivedAt == null);

            if (product == null)
            {
                return new Response<ProductDetails>
                {
                    Status = 404,
                    Message = "Produit non trouv�",
                    Data = null
                };
            }

            productDto.UpdateProduct(product);

            await context.SaveChangesAsync();

            // Recharger avec les relations pour la r�ponse
            var updatedProduct = await context.Products
                .Include(p => p.Cursus)
                .FirstAsync(p => p.Id == productDto.Id);

            return new Response<ProductDetails>
            {
                Status = 200,
                Message = "Produit mis � jour avec succ�s",
                Data = new ProductDetails(updatedProduct)
            };
        }
        catch (Exception ex)
        {
            return new Response<ProductDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la mise � jour du produit: {ex.Message}",
                Data = null
            };
        }
    }

    /// <summary>
    /// Archive un produit (suppression logique)
    /// </summary>
    /// <param name="id">Identifiant du produit</param>
    /// <returns>R�sultat de l'op�ration</returns>
    public async Task<Response<object>> DeleteProductAsync(Guid id)
    {
        try
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.ArchivedAt == null);

            if (product == null)
            {
                return new Response<object>
                {
                    Status = 404,
                    Message = "Produit non trouv�",
                    Data = null
                };
            }

            product.ArchivedAt = DateTimeOffset.UtcNow;
            product.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<object>
            {
                Status = 200,
                Message = "Produit supprim� avec succ�s",
                Data = null
            };
        }
        catch (Exception ex)
        {
            return new Response<object>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression du produit: {ex.Message}",
                Data = null
            };
        }
    }
}
