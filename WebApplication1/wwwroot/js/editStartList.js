let currentSourceHorseOrderId = null;

const modal = document.getElementById('editHorseModal');
modal.addEventListener('show.bs.modal', function (event) {
    const btn = event.relatedTarget;
    const horseOrderId = parseInt(btn.dataset.horseOrderId);
    const currentHorseId = btn.dataset.currentHorseId;
    const participant = btn.dataset.participant;
    const isTeam = btn.dataset.isTeam === 'true';
    const vaulterOrderId = btn.dataset.vaulterOrderId ? parseInt(btn.dataset.vaulterOrderId) : null;
    const stepId = btn.dataset.stepId ? parseInt(btn.dataset.stepId) : null;

    currentSourceHorseOrderId = horseOrderId;
    document.getElementById('modalParticipantLabel').textContent = participant;
    document.getElementById('modalHorseOrderId').value = horseOrderId;
    document.getElementById('modalHorseSelect').value = currentHorseId;

    const modeSelector = document.getElementById('modeSelector');
    if (isTeam) {
        modeSelector.style.display = 'none';
        showMode(1);
    } else {
        modeSelector.style.display = '';
        document.getElementById('modeAll').checked = true;
        document.getElementById('movingVaulterOrderId').value = vaulterOrderId;
        document.getElementById('movingVaulterNameHidden').value = participant;
        document.getElementById('mode3VaulterOrderId').value = vaulterOrderId;
        document.getElementById('mode3HorseSelect').value = currentHorseId;

        initStepDropdown(stepId);
        showMode(1);
    }
});

document.getElementById('modeAll').addEventListener('change', () => showMode(1));
document.getElementById('modeSingle').addEventListener('change', () => showMode(2));
document.getElementById('modeSplit').addEventListener('change', () => showMode(3));

document.getElementById('targetStepSelect').addEventListener('change', function () {
    const stepId = parseInt(this.value);
    document.getElementById('vaulterOrderInputs').innerHTML = '';
    if (!stepId) {
        document.getElementById('targetHorseOrderDiv').style.display = 'none';
        document.getElementById('targetHorseOrderSelect').innerHTML = '<option value="">-- Välj häst --</option>';
        return;
    }
    populateHorseDropdown(stepId);
    document.getElementById('targetHorseOrderDiv').style.display = '';
});

document.getElementById('targetHorseOrderSelect').addEventListener('change', function () {
    const targetId = parseInt(this.value);
    if (!targetId) {
        document.getElementById('vaulterOrderInputs').innerHTML = '';
        return;
    }
    const movingVaulterOrderId = parseInt(document.getElementById('movingVaulterOrderId').value);
    const movingVaulterName = document.getElementById('movingVaulterNameHidden').value;
    renderVaulterOrderInputs(targetId, currentSourceHorseOrderId, movingVaulterOrderId, movingVaulterName);
});

function showMode(mode) {
    document.getElementById('mode1Section').style.display = mode === 1 ? '' : 'none';
    document.getElementById('mode2Section').style.display = mode === 2 ? '' : 'none';
    document.getElementById('mode3Section').style.display = mode === 3 ? '' : 'none';
    document.getElementById('submitMode1').style.display = mode === 1 ? '' : 'none';
    document.getElementById('submitMode2').style.display = mode === 2 ? '' : 'none';
    document.getElementById('submitMode3').style.display = mode === 3 ? '' : 'none';
}

function initStepDropdown(currentStepId) {
    const stepSelect = document.getElementById('targetStepSelect');
    stepSelect.innerHTML = '<option value="">-- Välj omgång --</option>';
    allSteps.forEach(s => {
        const opt = document.createElement('option');
        opt.value = s.StepId;
        opt.textContent = s.Name;
        if (s.StepId === currentStepId) opt.selected = true;
        stepSelect.appendChild(opt);
    });

    if (currentStepId) {
        populateHorseDropdown(currentStepId);
        document.getElementById('targetHorseOrderDiv').style.display = '';
    } else {
        document.getElementById('targetHorseOrderDiv').style.display = 'none';
        document.getElementById('targetHorseOrderSelect').innerHTML = '<option value="">-- Välj häst --</option>';
    }
    document.getElementById('vaulterOrderInputs').innerHTML = '';
}

function populateHorseDropdown(stepId) {
    const select = document.getElementById('targetHorseOrderSelect');
    select.innerHTML = '<option value="">-- Välj häst --</option>';

    allHorseOrders
        .filter(ho => ho.StepId === stepId && ho.HorseOrderId !== currentSourceHorseOrderId)
        .sort((a, b) => a.StartNumber - b.StartNumber)
        .forEach(ho => {
            const opt = document.createElement('option');
            opt.value = ho.HorseOrderId;
            const vaulterPart = ho.Vaulters.length > 0
                ? ' – ' + ho.Vaulters.map(v => `${v.Order}. ${v.Name}`).join(', ')
                : '';
            opt.textContent = ho.Display + vaulterPart;
            select.appendChild(opt);
        });

    document.getElementById('vaulterOrderInputs').innerHTML = '';
}

function renderVaulterOrderInputs(targetHorseOrderId, sourceHorseOrderId, movingVaulterOrderId, movingVaulterName) {
    const targetHo = allHorseOrders.find(h => h.HorseOrderId === targetHorseOrderId);
    const sourceHo = allHorseOrders.find(h => h.HorseOrderId === sourceHorseOrderId);
    if (!targetHo) return;

    const sourceRemaining = (sourceHo ? sourceHo.Vaulters.filter(v => v.Id !== movingVaulterOrderId) : [])
        .slice().sort((a, b) => a.Order - b.Order);

    const targetVaulters = targetHo.Vaulters.map(v => ({ Id: v.Id, Name: v.Name, Order: v.Order }));
    targetVaulters.push({ Id: movingVaulterOrderId, Name: movingVaulterName, Order: targetVaulters.length + 1 });
    targetVaulters.sort((a, b) => a.Order - b.Order);

    let html = '';

    if (sourceRemaining.length > 0) {
        const sourceLabel = sourceHo ? sourceHo.Display : 'ursprunglig häst';
        html += `<p class="mt-3 mb-1 fw-semibold">Ny startordning på ${sourceLabel}:</p>`;
        html += '<table class="table table-sm"><thead><tr><th>Voltigör</th><th>Startordning</th></tr></thead><tbody>';
        sourceRemaining.forEach((v, idx) => {
            html += `<tr>
                <td>${v.Name}<input type="hidden" name="sourceVaulterOrderIds" value="${v.Id}"></td>
                <td><input type="number" name="sourceStartOrders" class="form-control form-control-sm" min="1" value="${idx + 1}" style="width:70px"></td>
            </tr>`;
        });
        html += '</tbody></table>';
    }

    html += `<p class="mt-3 mb-1 fw-semibold">Startordning på ${targetHo.Display}:</p>`;
    html += '<table class="table table-sm"><thead><tr><th>Voltigör</th><th>Startordning</th></tr></thead><tbody>';
    targetVaulters.forEach((v, idx) => {
        html += `<tr>
            <td>${v.Name}<input type="hidden" name="targetVaulterOrderIds" value="${v.Id}"></td>
            <td><input type="number" name="targetStartOrders" class="form-control form-control-sm" min="1" value="${idx + 1}" style="width:70px"></td>
        </tr>`;
    });
    html += '</tbody></table>';

    document.getElementById('vaulterOrderInputs').innerHTML = html;
}
