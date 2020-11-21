<template>
    <div class="home">
        <h1>{{ msg }}</h1>
        
      <b-row>
        <b-col lg="6" class="my-1">
          <b-form-group label="Find" label-cols-sm="6" label-align-sm="right" label-for="filterInput" class="mb-0">
              <b-input-group>
                <b-form-input
                  v-model="filter"
                  type="search"
                  placeholder="Type to Search"
                ></b-form-input>
                <b-input-group-append>
                  <b-button :disabled="!filter" @click="filter = ''">Clear</b-button>
                </b-input-group-append>
              </b-input-group>
            </b-form-group>
          </b-col>

          <b-col lg="6" class="my-1">
            <b-pagination
              v-model="page"
              :total-rows="recordCount"
              :per-page="perPage"
              aria-controls="tblBooks">
            </b-pagination>
          </b-col>
        </b-row>

        <b-table id="tblBooks" striped hover :items="dataContext" :fields="fields" responsive="sm" :per-page="perPage" :current-page="page" :busy.sync="isBusy"
                :filter="filter"
                @filtered="onFiltered" >
            <template v-slot:cell(thumbnailUrl)="data">
                <b-img :src="data.value" thumbnail fluid></b-img>
            </template>
            <template v-slot:cell(title_link)="data">
                <b-link :to="{ name: 'book_view', params: { 'id' : data.item.bookId } }">{{ data.item.title }}</b-link>
            </template>
        </b-table>
    </div>
</template>

<script>
    import axios from 'axios';

  export default {
    name: 'Home',
    props: {
      msg: String
    },
    data: () => ({
      fields: [
        { key: 'thumbnailUrl', label: 'Book Image' },
        { key: 'title_link', label: 'Book Title', sortable: true, sortDirection: 'desc' },
        { key: 'isbn', label: 'ISBN', sortable: true, sortDirection: 'desc' },
        { key: 'descr', label: 'Description', sortable: true, sortDirection: 'desc' }

      ],
      page: 1,
      perPage: 10,
      recordCount: 0,
      isBusy: false,
      filter: null,
    }),
    methods: {
      dataContext(ctx, callback) {
        this.isBusy = true;

        axios.post('https://localhost:5001/books', ctx)
          .then(response => {
            this.recordCount = response.data.recordCount;
            callback(response.data.books);
          })
          .catch(() => {
            callback([]);
          }).finally(() => {
            this.isBusy = false;
          });
      },
      onFiltered(filteredItems) {
        this.recordCount = filteredItems.length;
        this.page = 1;
      }
    }
  };
</script>