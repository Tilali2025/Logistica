// -------------------------
// TOAST
// -------------------------
function mostrarToast(mensaje, tipo = 'success') {
    const toastId = 'toast-' + Date.now();

    const html = `
        <div id="${toastId}" class="toast align-items-center text-bg-${tipo} border-0 mb-2"
             role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">${mensaje}</div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto"
                        data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;

    $('#toastContainer').append(html);

    const toastEl = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastEl, { delay: 3000 });
    toast.show();

    toastEl.addEventListener('hidden.bs.toast', () => toastEl.remove());
}


function abrirModal(button) {
    if (!button) return;
    const codCia = button.getAttribute('data-codcia')
    alert('CodCia: ' + codCia);

    const url = codCia
        ? '/Movimiento/Editar?id=' + encodeURIComponent(codCia)
        : '/Movimiento/Crear';

    $.get(url, function (html) {
        $('#modalContainer').html(html);

        const modalEl = document.getElementById('movimientoModal');
        if (!modalEl) return;

        const modal = new bootstrap.Modal(modalEl);
        modal.show();

        $('#formMovimiento').off('submit').on('submit', function (e) {
            e.preventDefault();
            guardarMovimiento();
        });
    });
}


// -------------------------
// GUARDAR MOVIMIENTO
// -------------------------
function guardarMovimiento() {
    const form = $('#formMovimiento');
    const codCia = form.find('[name="CodCia"]').val();

    if (!codCia) {
        mostrarToast('Debe ingresar el Código de Compañía', 'danger');
        return;
    }

    $.post({
        url: '/Movimiento/Guardar',
        data: form.serialize(),
        success: function (resp) {
            if (resp.success) {
                mostrarToast(resp.message, 'success');
                setTimeout(() => location.reload(), 1500);
            } else {
                mostrarToast('No se pudo guardar', 'danger');
            }
        },
        error: function (xhr) {
            mostrarToast(xhr.responseText || 'Error al guardar', 'danger');
        }
    });
}

// -------------------------
// ELIMINAR MOVIMIENTO
// -------------------------
function eliminarModal(button) {
    const id = button.getAttribute('data-codcia');
    if (!id) return;

    if (!confirm('¿Seguro que deseas eliminar este movimiento?')) return;

    $.post({
        url: `/Movimiento/Eliminar/${id}`,
        success: function (resp) {
            if (resp.success) {
                mostrarToast(resp.message, 'success');
                setTimeout(() => location.reload(), 1000);
            } else {
                mostrarToast('No se pudo eliminar', 'danger');
            }
        },
        error: function () {
            mostrarToast('Error al eliminar', 'danger');
        }
    });
}
