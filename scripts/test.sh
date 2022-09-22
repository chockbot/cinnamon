echo "${{ secrets.SERVER_SSH_KEY }}" | tr -d '\r' > key.pem
         chmod 400 key.pem

cat key.pem