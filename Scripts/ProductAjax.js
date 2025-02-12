document.addEventListener("DOMContentLoaded", function () {
  document.querySelectorAll(".delete-btn").forEach(button => {
    button.addEventListener("click", async function () {
      const form = this.closest(".product-delete-form");
      const productId = form.dataset.id;
      const csrfToken = form.querySelector("input[name='__RequestVerificationToken']").value;

      const confirmDelete = confirm("Are you sure you want to delete this product?");
      if (!confirmDelete) return;

      try {
        const response = await fetch(`/products/${productId}`, {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            "X-CSRF-TOKEN": csrfToken 
          }
        });

        const result = await response.json();

        if (result.success) {
          alert(result.message);
          window.location.href = '/';
          form.closest(".product-item")?.remove();
        } else {
          alert("Error: " + result.message);
        }
      } catch (error) {
        console.error("Network error:", error);
        alert("Error: Something went wrong.");
      }
    });
  });
});
