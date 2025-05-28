const API_BASE_URL = 'http://localhost:5252';
let categories = [];
let subcategories = [];

// Check if the user is logged in by looking for a 'user' object in local storage
function isLoggedIn() {
    return localStorage.getItem('user') !== null;
}

// Toggle the visibility of the "Add Contact" button based on login status
function checkAuth() {
    const addContactBtn = document.getElementById('add-contact-button');
    if (isLoggedIn()) {
        addContactBtn?.classList.remove('hidden');
    } else {
        addContactBtn?.classList.add('hidden');
    }
}

// Load all contacts from the backend and render them as a list
async function fetchContacts() {
    const loadingState = document.getElementById('loading-state');
    const contactListDiv = document.getElementById('contact-list');

    loadingState.classList.remove('hidden');
    contactListDiv.innerHTML = '';

    try {
        const response = await fetch(`${API_BASE_URL}/api/Contacts`);
        const contacts = await response.json();

        loadingState.classList.add('hidden');

        if (!contacts.length) {
            contactListDiv.innerHTML = '<div class="empty-state">Brak kontaktów</div>';
            return;
        }

        // Generate HTML for each contact in the list
        contacts.forEach(contact => {
            const item = document.createElement('div');
            item.className = 'contact-item';
            item.innerHTML = `
        <div>
          <h3>${contact.imie} ${contact.nazwisko}</h3>
          <p>${contact.email}</p>
        </div>
        <span>${contact.kategoria ? contact.kategoria.nazwa : 'Brak kategorii'}</span>
      `;
            // Clicking a contact loads its full details
            item.onclick = () => displayContactDetails(contact.id);
            contactListDiv.appendChild(item);
        });
    } catch (error) {
        console.error('Error:', error);
        contactListDiv.innerHTML = `<div class="empty-state text-red-500">Failed to load contacts</div>`;
    }
}

// Fetch detailed data for a specific contact and display it in the sidebar
async function displayContactDetails(contactId) {
    const detailsDiv = document.getElementById('contact-details');
    const detailsContentDiv = document.getElementById('details-content');

    detailsDiv.classList.remove('hidden');
    detailsContentDiv.innerHTML = '<p class="text-center text-gray-500">Loading details...</p>';

    try {
        const user = JSON.parse(localStorage.getItem('user'));
        const token = user?.token;

        if (!token) throw new Error("Missing authorization token");

        const response = await fetch(`${API_BASE_URL}/api/Contacts/${contactId}`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

        const contact = await response.json();

        // Render contact details, handling possible nulls and formatting the date
        detailsContentDiv.innerHTML = `
      <div class="detail-row"><span class="detail-label">Imie:</span><span class="detail-value">${contact.imie}</span></div>
      <div class="detail-row"><span class="detail-label">Nazwisko:</span><span class="detail-value">${contact.nazwisko}</span></div>
      <div class="detail-row"><span class="detail-label">Email:</span><span class="detail-value">${contact.email}</span></div>
      <div class="detail-row"><span class="detail-label">Kategoria:</span><span class="detail-value">${contact.kategoria ? contact.kategoria.nazwa : 'Brak'}</span></div>
      <div class="detail-row"><span class="detail-label">Podkategoria:</span><span class="detail-value">${contact.podkategoria ? contact.podkategoria.nazwa : 'Brak'}</span></div>
      <div class="detail-row"><span class="detail-label">Telefon:</span><span class="detail-value">${contact.telefon || 'Brak'}</span></div>
      <div class="detail-row"><span class="detail-label">Data urodzenia:</span><span class="detail-value">${contact.dataUrodzenia ? new Date(contact.dataUrodzenia).toLocaleDateString('pl-PL') : 'Brak'}</span></div>
    `;
    } catch (error) {
        console.error('Error loading contact details:', error);
        detailsContentDiv.innerHTML = `<p class="text-center text-red-500">Failed to load contact details</p>`;
    }
}

// Handle user login – send credentials to the backend and store the user info
async function handleLogin(event) {
    event.preventDefault();

    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    const response = await fetch(`${API_BASE_URL}/api/Auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, haslo: password })
    });

    const errorText = document.getElementById('login-error');

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem('user', JSON.stringify(data)); // store token for future requests
        errorText.classList.add('hidden');
        checkAuth(); // update UI
    } else {
        errorText.textContent = 'Invalid email or password';
        errorText.classList.remove('hidden');
    }
}

// Submit form to create a new contact
async function saveContact(event) {
    event.preventDefault();

    if (!isLoggedIn()) {
        alert("You must be logged in to add a contact.");
        return;
    }

    // Create contact object from form values
    const contact = {
        imie: document.getElementById('imie').value,
        nazwisko: document.getElementById('nazwisko').value,
        email: document.getElementById('email').value,
        hasloHash: document.getElementById('haslo').value,
        kategoriaId: parseInt(document.getElementById('kategoria-id').value),
        telefon: document.getElementById('telefon').value,
        dataUrodzenia: document.getElementById('data-urodzenia').value
            ? new Date(document.getElementById('data-urodzenia').value)
            : null
    };

    const selectedCategoryId = contact.kategoriaId;

    // Depending on selected category, assign subcategory or custom name
    if ((selectedCategoryId === 1 || selectedCategoryId === 2) && document.getElementById('podkategoria-id')) {
        contact.podkategoriaId = parseInt(document.getElementById('podkategoria-id').value);
    } else if (selectedCategoryId === 3 && document.getElementById('podkategoria-inna')) {
        contact.podkategoriaInna = document.getElementById('podkategoria-inna').value;
    }

    const user = JSON.parse(localStorage.getItem('user'));
    const token = user?.token;

    // Send contact data to backend
    const response = await fetch(`${API_BASE_URL}/api/Contacts`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(contact)
    });

    if (response.ok) {
        hideForm();     // Hide form after successful save
        fetchContacts(); // Refresh contact list
    } else {
        alert("Failed to save contact");
    }
}

// Show the form to add a new contact
function showAddForm() {
    if (!isLoggedIn()) {
        alert("You must be logged in to add a contact.");
        return;
    }

    document.getElementById('form-title').textContent = 'Add New Contact';
    document.getElementById('contact-edit-form').reset();
    document.getElementById('contact-id').value = '';
    document.getElementById('delete-button')?.classList.add('hidden');
    document.getElementById('contact-form').classList.remove('hidden');
}

// Hide the add/edit contact form
function hideForm() {
    document.getElementById('contact-form').classList.add('hidden');
}

// Fetch all categories and subcategories used to populate <select> inputs
async function fetchCategoriesAndSubcategories() {
    const catRes = await fetch(`${API_BASE_URL}/api/ContactCategories`);
    const subcatRes = await fetch(`${API_BASE_URL}/api/ContactSubcategories`);

    categories = await catRes.json();
    subcategories = await subcatRes.json();

    // Populate category dropdown with options
    const select = document.getElementById('kategoria-id');
    select.innerHTML = categories.map(cat => `<option value="${cat.id}">${cat.nazwa}</option>`).join('');
}

// Populate the subcategory dropdown (used if category = 1 or 2)
async function loadSubcategoriesIntoForm() {
    const podkategoriaSelect = document.getElementById('podkategoria-id');
    if (!podkategoriaSelect) return;
    podkategoriaSelect.innerHTML = subcategories.map(sc => `<option value="${sc.id}">${sc.nazwa}</option>`).join('');
}

// Toggle subcategory dropdown or input visibility based on selected category
function setupCategoryChangeHandler() {
    const kategoriaSelect = document.getElementById('kategoria-id');
    const podkategoriaContainer = document.getElementById('podkategoria-container');
    const podkategoriaInputContainer = document.getElementById('podkategoria-input-container');

    kategoriaSelect.addEventListener('change', () => {
        const id = parseInt(kategoriaSelect.value);
        // Show dropdown for categories 1, hide otherwise
        podkategoriaContainer.classList.toggle('hidden', id !== 1);
        // Show free-text input for category 3 only
        podkategoriaInputContainer.classList.toggle('hidden', id !== 3);
    });
}

// Initialization logic after DOM is fully loaded
document.addEventListener('DOMContentLoaded', () => {
    fetchCategoriesAndSubcategories()
        .then(loadSubcategoriesIntoForm)
        .then(setupCategoryChangeHandler);

    fetchContacts(); // Load initial contact list
    checkAuth();     // Show/hide elements depending on auth
    document.getElementById('login')?.addEventListener('submit', handleLogin);
    document.getElementById('contact-edit-form')?.addEventListener('submit', saveContact);
});
