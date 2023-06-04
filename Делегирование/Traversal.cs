using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            var products = new List<Product>();
            TraverseProductCategories(root, pc => products.AddRange(pc.Products));
            return products;
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            var jobs = new List<Job>();
            TraverseJobs(root, j =>
            {
                if (j.Subjobs.Count == 0)
                    jobs.Add(j);
            });
            return jobs;
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            var values = new List<T>();
            TraverseBinaryTree(root, node =>
            {
                if (node.Left == null && node.Right == null)
                    values.Add(node.Value);
            });
            return values;
        }

        private static void TraverseProductCategories(ProductCategory category, Action<ProductCategory> action)
        {
            action(category);
            foreach (var subcategory in category.Categories)
                TraverseProductCategories(subcategory, action);
        }

        private static void TraverseJobs(Job job, Action<Job> action)
        {
            action(job);
            foreach (var subjob in job.Subjobs)
                TraverseJobs(subjob, action);
        }

        private static void TraverseBinaryTree<T>(BinaryTree<T> node, Action<BinaryTree<T>> action)
        {
            action(node);
            if (node.Left != null)
                TraverseBinaryTree(node.Left, action);
            if (node.Right != null)
                TraverseBinaryTree(node.Right, action);
        }
    }
}