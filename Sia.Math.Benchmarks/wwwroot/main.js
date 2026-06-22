import { dotnet } from './_framework/dotnet.js'

const { setModuleImports, getAssemblyExports, runMain } = await dotnet.create();

setModuleImports('main.js', {
    initProgress(total) {
        const bar = document.getElementById('progress-bar');
        const text = document.getElementById('progress-text');
        if (bar) bar.style.width = '0%';
        if (text) text.textContent = `0 / ${total}`;
    },
    updateProgress(current, total) {
        const bar = document.getElementById('progress-bar');
        const text = document.getElementById('progress-text');
        if (bar) {
            const percent = total > 0 ? (current / total) * 100 : 0;
            bar.style.width = percent + '%';
        }
        if (text) {
            text.textContent = `${current} / ${total}`;
        }
    },
    addGroup(name) {
        const div = document.createElement('div');
        div.className = 'group';
        div.innerHTML = `<h2>${name}</h2><table><thead><tr>
            <th>Test</th><th>Mean (ns)</th><th>Ratio</th>
        </tr></thead><tbody></tbody></table>`;
        document.getElementById('results').appendChild(div);
    },
    addResult(name, meanNs, ratio) {
        const groups = document.querySelectorAll('.group');
        const tbody = groups[groups.length - 1].querySelector('tbody');
        const cls = ratio < 0.95 ? 'win' : ratio > 1.05 ? 'loss' : 'draw';
        const ratioText = ratio.toFixed(2) + '\xd7';
        tbody.insertAdjacentHTML('beforeend',
            `<tr>
                <td>${name}</td>
                <td>${meanNs.toFixed(1)}</td>
                <td class="ratio ${cls}">${ratioText}</td>
            </tr>`);
    },
    setStatus(text) {
        document.getElementById('status').textContent = text;
    },
});

await runMain();

const runBtn = document.getElementById('run-btn');
const exports = await getAssemblyExports('Sia.Math.Benchmarks.Browser.dll');
const runAll = exports.Sia.Math.Benchmarks.BenchmarkRunner.RunAll;

async function run() {
    document.getElementById('results').textContent = '';
    runBtn.disabled = true;
    try {
        await runAll();
    } finally {
        runBtn.disabled = false;
    }
}

runBtn.addEventListener('click', run);
