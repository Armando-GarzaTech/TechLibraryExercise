<template>
    <div v-if="book">
        <b-card :title="book.title"
                :img-src="book.thumbnailUrl"
                img-alt="Image"
                img-top
                tag="article"
                style="max-width: 30rem;"
                class="mb-2">
            <b-card-text style="min-height: 150px;">
                <div v-if="editing">
                  <b-form-textarea v-model="book.descr" rows="4">
                  </b-form-textarea> 
                </div>
                <div v-else class="textbox">
                  {{ book.descr }}
                </div>
            </b-card-text>

          <b-row align-v="center">
            <b-col>
              <b-button to="/" variant="primary">Back</b-button>
              <b-button variant="success" @click="save" v-if="editing">Save</b-button>
            </b-col>
            <b-col><b-form-checkbox v-model="editing" switch size="lg">Edit</b-form-checkbox></b-col>
          </b-row>
        </b-card>
    </div>
</template>

<script>
    import axios from 'axios';

  export default {
    name: 'Book',
    props: ["id"],
    data: () => ({
      book: null,
      editing: false,
    }),
    mounted() {
      axios.get(`https://localhost:5001/books/${this.id}`)
        .then(response => {
          this.book = response.data;
        });
    },
    methods: {
      save() {
        axios.post(`https://localhost:5001/books/update`, this.book)
          .then(() => {
            window.history.length > 1 ? this.$router.go(-1) : this.$router.push('/');
          });
      },
    }

  }
</script>

<style scoped>
  .textbox {
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
  }

  .btn {
    width: 75px;
    margin-right: 6px;
  }
</style>