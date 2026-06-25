namespace TeamsNotifier.Web;

public static class HtmlPage
{
    public static string Render() => """
<!DOCTYPE html>
<html lang="vi">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>Teams Notifier</title>
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  body {
    font-family: 'Segoe UI', system-ui, sans-serif;
    background: #0f0f14;
    color: #e8e8f0;
    min-height: 100vh;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 40px 20px;
  }
  header { display: flex; align-items: center; gap: 14px; margin-bottom: 36px; }
  .logo {
    width: 42px; height: 42px; background: #6264a7;
    border-radius: 10px; display: flex; align-items: center;
    justify-content: center; font-size: 22px;
  }
  h1 { font-size: 1.6rem; font-weight: 700; letter-spacing: -0.5px; }
  h1 span { color: #7b83eb; }
  .container { width: 100%; max-width: 860px; display: grid; grid-template-columns: 1fr 1fr; gap: 20px; }
  @media (max-width: 640px) { .container { grid-template-columns: 1fr; } }
  .card {
    background: #1a1a24; border: 1px solid #2a2a38;
    border-radius: 14px; padding: 24px;
    display: flex; flex-direction: column; gap: 14px;
  }
  .card-title { font-size: 0.75rem; font-weight: 600; letter-spacing: 1.2px; text-transform: uppercase; color: #7b83eb; }
  label { font-size: 0.8rem; color: #9090a8; margin-bottom: 4px; display: block; }
  input, textarea {
    width: 100%; background: #0f0f14; border: 1px solid #2a2a38;
    border-radius: 8px; color: #e8e8f0; font-size: 0.9rem;
    font-family: inherit; padding: 9px 12px; outline: none; transition: border-color .2s;
  }
  input:focus, textarea:focus { border-color: #6264a7; }
  textarea { resize: vertical; min-height: 80px; }
  .severity-row { display: flex; gap: 8px; }
  .sev-btn {
    flex: 1; padding: 7px 4px; border-radius: 8px;
    border: 1.5px solid #2a2a38; background: transparent;
    color: #9090a8; font-size: 0.78rem; font-weight: 600;
    cursor: pointer; transition: all .15s; text-align: center;
  }
  .sev-btn:hover { border-color: #6264a7; color: #e8e8f0; }
  .sev-btn.active-info    { border-color: #3d9e6e; background: #1a3028; color: #5dba8a; }
  .sev-btn.active-warning { border-color: #c8892a; background: #2e2010; color: #e6a835; }
  .sev-btn.active-error   { border-color: #c84040; background: #2e1010; color: #e05050; }
  .extra-row { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; margin-bottom: 6px; }
  .add-extra {
    background: transparent; border: 1px dashed #2a2a38;
    color: #7b83eb; border-radius: 8px; padding: 8px;
    font-size: 0.8rem; cursor: pointer; width: 100%; transition: border-color .2s;
  }
  .add-extra:hover { border-color: #6264a7; }
  .send-btn {
    width: 100%; padding: 12px; border-radius: 10px; border: none;
    background: #6264a7; color: #fff; font-size: 0.95rem; font-weight: 700;
    cursor: pointer; transition: background .15s, transform .1s; margin-top: 4px;
  }
  .send-btn:hover  { background: #7b83eb; }
  .send-btn:active { transform: scale(0.98); }
  .send-btn:disabled { background: #333; color: #666; cursor: not-allowed; }
  #toast {
    position: fixed; bottom: 30px; left: 50%;
    transform: translateX(-50%) translateY(20px);
    background: #1a1a24; border: 1px solid #2a2a38;
    border-radius: 10px; padding: 12px 22px;
    font-size: 0.9rem; opacity: 0; pointer-events: none;
    transition: all .3s; max-width: 360px; text-align: center;
  }
  #toast.show { opacity: 1; transform: translateX(-50%) translateY(0); }
  #toast.success { border-color: #3d9e6e; color: #5dba8a; }
  #toast.error   { border-color: #c84040; color: #e05050; }
  .field { display: flex; flex-direction: column; gap: 4px; }
</style>
</head>
<body>

<header>
  <div class="logo">💬</div>
  <h1>Teams <span>Notifier</span></h1>
</header>

<div class="container">
  <div class="card">
    <div class="card-title">✉️ Custom Message</div>
    <div class="field">
      <label>Tiêu đề</label>
      <input id="msg-title" type="text" placeholder="Deploy thành công 🚀" />
    </div>
    <div class="field">
      <label>Nội dung</label>
      <textarea id="msg-body" placeholder="Nhập nội dung message..."></textarea>
    </div>
    <div class="field">
      <label>Severity</label>
      <div class="severity-row">
        <button class="sev-btn active-info" data-sev="Info"    onclick="setSev(this)">ℹ️ Info</button>
        <button class="sev-btn"            data-sev="Warning" onclick="setSev(this)">⚠️ Warning</button>
        <button class="sev-btn"            data-sev="Error"   onclick="setSev(this)">🔴 Error</button>
      </div>
    </div>
    <div class="field">
      <label>URL đính kèm (tuỳ chọn)</label>
      <input id="msg-url" type="url" placeholder="https://github.com/..." />
    </div>
    <button class="send-btn" onclick="sendMessage(event)">Gửi lên Teams →</button>
  </div>

  <div class="card">
    <div class="card-title">🔴 Error / Exception Alert</div>
    <div class="field">
      <label>Error message</label>
      <input id="err-msg" type="text" placeholder="NullReferenceException: Object reference not set..." />
    </div>
    <div class="field">
      <label>Context (service / method)</label>
      <input id="err-ctx" type="text" placeholder="OrderService.PlaceOrderAsync" />
    </div>
    <div class="field">
      <label>User ID (tuỳ chọn)</label>
      <input id="err-uid" type="text" placeholder="user-42" />
    </div>
    <div class="field">
      <label>Extra Data</label>
      <div id="extra-fields"></div>
      <button class="add-extra" onclick="addExtra()">+ Thêm key / value</button>
    </div>
    <button class="send-btn" onclick="sendError(event)">Gửi Error Alert →</button>
  </div>
</div>

<div id="toast"></div>

<script>
  let currentSev = 'Info';

  function setSev(btn) {
    document.querySelectorAll('.sev-btn').forEach(b => b.className = 'sev-btn');
    const map = { Info: 'active-info', Warning: 'active-warning', Error: 'active-error' };
    btn.classList.add(map[btn.dataset.sev]);
    currentSev = btn.dataset.sev;
  }

  function addExtra() {
    const div = document.createElement('div');
    div.className = 'extra-row';
    div.innerHTML = '<input type="text" placeholder="key" class="extra-key" /><input type="text" placeholder="value" class="extra-val" />';
    document.getElementById('extra-fields').appendChild(div);
  }

  function showToast(msg, type) {
    const t = document.getElementById('toast');
    t.textContent = msg;
    t.className = 'show ' + type;
    setTimeout(() => { t.className = ''; }, 3200);
  }

  async function sendMessage(event) {
    const btn   = event.target;
    const title = document.getElementById('msg-title').value.trim();
    const body  = document.getElementById('msg-body').value.trim();
    const url   = document.getElementById('msg-url').value.trim();
    if (!title || !body) return showToast('Vui lòng nhập Tiêu đề và Nội dung', 'error');
    btn.disabled = true; btn.textContent = 'Đang gửi...';
    try {
      const res  = await fetch('/send/message', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ title, body, severity: currentSev, url: url || null }) });
      const data = await res.json();
      showToast(data.message, data.ok ? 'success' : 'error');
    } catch { showToast('Lỗi kết nối tới server', 'error'); }
    finally { btn.disabled = false; btn.textContent = 'Gửi lên Teams →'; }
  }

  async function sendError(event) {
    const btn     = event.target;
    const message = document.getElementById('err-msg').value.trim();
    const context = document.getElementById('err-ctx').value.trim();
    const userId  = document.getElementById('err-uid').value.trim();
    if (!message) return showToast('Vui lòng nhập Error message', 'error');
    const extraData = {};
    document.querySelectorAll('#extra-fields .extra-row').forEach(row => {
      const k = row.querySelector('.extra-key').value.trim();
      const v = row.querySelector('.extra-val').value.trim();
      if (k) extraData[k] = v;
    });
    btn.disabled = true; btn.textContent = 'Đang gửi...';
    try {
      const res  = await fetch('/send/error', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ message, context: context || null, userId: userId || null, extraData }) });
      const data = await res.json();
      showToast(data.message, data.ok ? 'success' : 'error');
    } catch { showToast('Lỗi kết nối tới server', 'error'); }
    finally { btn.disabled = false; btn.textContent = 'Gửi Error Alert →'; }
  }
</script>
</body>
</html>
""";
}
